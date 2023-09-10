namespace RedditStats.Models;

public class RateLimit
{
    public int RemainingCalls { get; set; }
    public int ResetSeconds { get; set; }
}