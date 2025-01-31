using IMDbCrawler.Models;
using IMDbCrawler.Services;
using IMDbCrawler.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Configurações
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<ImdbScraperOptions>(
    builder.Configuration.GetSection("ImdbScraper"));

// Serviços
builder.Services.AddHttpClient<IImdbScraper, ImdbScraper>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ImdbScraperOptions>>().Value;

    // Configurações do HttpClient
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
    client.Timeout = TimeSpan.FromSeconds(30); // Timeout de 30 segundos
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var scraper = host.Services.GetRequiredService<IImdbScraper>();

try
{
    logger.LogInformation("Starting scraping process...");
    var movies = await scraper.ScrapeTopMoviesAsync(20);

    CsvExporter.ExportToFile(movies, "movies.csv");
    logger.LogInformation($"Successfully exported {movies.Count} movies to movies.csv");
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Fatal error occurred during scraping process");
}