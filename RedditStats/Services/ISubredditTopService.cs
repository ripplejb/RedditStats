using RedditStats.Models;

namespace RedditStats.Services;

public interface ISubredditTopService
{
    Task<RedditResponse> Call(int topCount, string period);
    
}