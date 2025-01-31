using IMDbCrawler.Models;

namespace IMDbCrawler.Services;
public interface IImdbScraper
{
    Task<List<Movie>> ScrapeTopMoviesAsync(int count);
}
