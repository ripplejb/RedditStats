namespace RedditStats.Models;

public class RedditResponse
{
    public RateLimit RateLimit { get; set; }
    public SubredditData? SubredditData { get; set; }
}