using RedditStats.Models;

namespace RedditStats.Services;

public interface ISubredditCommentService
{
    Task<RedditResponse> Call(string uri);
}