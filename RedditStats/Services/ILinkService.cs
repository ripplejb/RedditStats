using RedditStats.Models;

namespace RedditStats.Services;

public interface ILinkService
{
    List<SubredditStatistic> GetResult(RedditResponse redditResponse);
}