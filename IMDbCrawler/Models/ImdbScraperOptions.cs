namespace IMDbCrawler.Models;
public class ImdbScraperOptions
{
    public int RequestDelayMs { get; set; } = 1000; // Delay entre requisições
    public int MaxConcurrentRequests { get; set; } = 3; // Limite de requisições simultâneas
}
