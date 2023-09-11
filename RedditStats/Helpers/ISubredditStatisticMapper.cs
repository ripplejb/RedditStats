using RedditStats.Models;

namespace RedditStats.Helpers;

public interface ISubredditStatisticMapper
{
    SubredditStatistic MapSubredditData(SubredditData subredditData);
}