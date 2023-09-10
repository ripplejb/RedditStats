using RedditStats.Models;

namespace RedditStats.Services;

public interface ISubredditService
{
    Task<RedditResponse> Call(string uri);
    
}