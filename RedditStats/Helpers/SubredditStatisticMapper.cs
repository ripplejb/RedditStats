using RedditStats.Models;

namespace RedditStats.Helpers;

public class SubredditStatisticMapper : ISubredditStatisticMapper
{
    public SubredditStatistic MapSubredditData(SubredditData subredditData)
    {
        var subredditStatistic = new SubredditStatistic
        {
            Kind = subredditData.Kind,
            CreateTime = DateTime.Now,
            Author = subredditData.Data["author"]?.GetValue<string>() ?? string.Empty,
            Id = subredditData.Data["id"]?.GetValue<string>() ?? string.Empty,
            UpVotes = subredditData.Data["ups"]?.GetValue<int>() ?? 0
        };
        return subredditStatistic;
    }
}