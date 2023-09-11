using RedditStats.Models;

namespace RedditStats.Services;

public interface ILinkService
{
    Tuple<List<string>, List<SubredditStatistic>> GetResult(RedditResponse redditResponse);
}