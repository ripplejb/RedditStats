namespace RedditStats.Models;

public class RedditResponse
{
    public RateLimit RateLimit { get; set; }
    public List<SubredditData> SubredditData { get; set; }
}