using System.Net;
using HtmlAgilityPack;
using IMDbCrawler.Models;
using IMDbCrawler.Utils;
using Microsoft.Extensions.Logging;
using Polly;

namespace IMDbCrawler.Services;

public class ImdbScraper : IImdbScraper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ImdbScraper> _logger;
    private readonly AsyncPolicy _retryPolicy;

    public ImdbScraper(HttpClient httpClient, ILogger<ImdbScraper> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        //_retryPolicy = retryPolicy;

        // Configura política de retry com backoff exponencial
        _retryPolicy = Policy
            .Handle<HttpRequestException>(ex => ex.StatusCode != HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, delay, retryCount, context) =>
                {
                    _logger.LogWarning($"Retry {retryCount} due to error: {exception.Message}");
                });
    }

    public async Task<List<Movie>> ScrapeTopMoviesAsync(int count)
    {
        var mainPageContent = await _retryPolicy.ExecuteAsync(() =>
            _httpClient.GetStringAsync(ImdbPaths.Top250Url));

        var movieLinks = ParseMovieLinks(mainPageContent, count);
        var movies = new List<Movie>();

        // Processamento paralelo controlado
        var semaphore = new SemaphoreSlim(initialCount: 3); // Limite de 3 requests simultâneas
        var tasks = movieLinks.Select(async link =>
        {
            await semaphore.WaitAsync();
            try
            {
                var movie = await ProcessMoviePage(link);
                return movie;
            }
            finally
            {
                semaphore.Release();
            }
        });

        movies.AddRange(await Task.WhenAll(tasks));
        return movies.ToList();
    }

    private List<string> ParseMovieLinks(string html, int count)
    {
        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Seleciona os primeiros 'count' elementos da lista
            return doc.DocumentNode
                .SelectNodes(ImdbSelectors.MovieLinkNode)
                ?.Take(count)
                .Select(node =>
                    ImdbPaths.BaseUrl + node?.Attributes[0]?.Value)
                .ToList() ?? new List<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao analisar links de filmes.");
            return new List<string>();
        }
    }

    private async Task<Movie> ProcessMoviePage(string url)
    {
        try
        {
            var html = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetStringAsync(url));

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return new Movie
            {
                Title = doc.DocumentNode.SelectSingleNode(ImdbSelectors.TitleNode).InnerText.Trim(),
                Year = doc.DocumentNode.SelectSingleNode(ImdbSelectors.YearNode).InnerText.Trim().Trim('(', ')', ' '),
                Director = doc.DocumentNode.SelectSingleNode(ImdbSelectors.DirectorNode).InnerText.Trim(),
                Rating = doc.DocumentNode.SelectSingleNode(ImdbSelectors.RatingNode).InnerText.Trim(),
                NumberOfReviews = doc.DocumentNode.SelectSingleNode(ImdbSelectors.ReviewsNode).InnerText.Trim().Replace("&nbsp;", "")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing movie page: {url}");
            return null;
        }
    }
}
