namespace RedditStats.Configs;

public class RedditConfigs
{
    public string Credentials { get; set; } = string.Empty;
    public string NoAuthBaseUrl { get; set; } = string.Empty;
    public string AuthBaseUrl { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
    public string Subreddit { get; set; } = string.Empty;
}