namespace IMDbCrawler.Utils;

public static class ImdbSelectors
{
    // Seletores para a lista de filmes
    public const string MovieListNodes = "//*[@id='__next']/main/div/div[3]/section/div/div[2]/div/ul/li";
    public const string MovieLinkNode = ".//a[@class='ipc-title-link-wrapper']";

    // Seletores para a página de detalhes do filme
    public const string TitleNode = "//h1[@data-testid='hero__pageTitle']//span[@data-testid='hero__primary-text']";
    public const string YearNode = "//*[@id=\"__next\"]/main/div/section[1]/section/div[3]/section/section/div[2]/div[1]/ul/li[1]/a";
    public const string DirectorNode = "//div[@class='ipc-metadata-list-item__content-container']//a[contains(@href, '/name/')]";
    public const string RatingNode = "//div[@data-testid='hero-rating-bar__aggregate-rating']//span[@class='sc-d541859f-1 imUuxf']";
    public const string ReviewsNode = "//div[@data-testid='hero-rating-bar__aggregate-rating']//div[@class='sc-d541859f-3 dwhNqC']";
}

public static class ImdbPaths
{
    public const string BaseUrl = "https://www.imdb.com";
    public const string Top250Url = BaseUrl + "/chart/top/";
}