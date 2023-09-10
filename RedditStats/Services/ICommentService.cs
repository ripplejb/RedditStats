using RedditStats.Models;

namespace RedditStats.Services;

public interface ICommentService
{
    Tuple<List<string>, List<SubredditStatistic>> GetResult(RedditResponse redditResponse);
}