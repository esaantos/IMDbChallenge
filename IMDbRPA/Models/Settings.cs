namespace IMDbRPA.Models;

public class Settings
{
    public string LoginUrl { get; set; }
    public string RatingsUrl { get; set; }
    public Credentials Credentials { get; set; }
    public BrowserOptions BrowserOptions { get; set; }

    public Delays Delays { get; set; }
}
public class Credentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class BrowserOptions
{
    public bool Headless { get; set; }
    public bool Maximized { get; set; }
}

public class Delays
{
    public int MinSeconds { get; set; }
    public int MaxSeconds { get; set; }
}