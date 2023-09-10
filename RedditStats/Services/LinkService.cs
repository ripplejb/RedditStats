using System.Text.Json;
using RedditStats.Models;

namespace RedditStats.Services;

public class LinkService : ILinkService
{
    public List<SubredditStatistic> GetResult(RedditResponse redditResponse)
    {
        var stats = new List<SubredditStatistic>();

        if (redditResponse.SubredditData.Count == 0) return stats;
        foreach (var sd in redditResponse.SubredditData)
        {
            if (sd.Kind != "Listing") return stats;

            var children = sd.Data["children"].Deserialize<List<SubredditData>>();
            if (children == null) return stats;
        
            stats.AddRange(from rd in children where rd.Kind == "t3" select GetSubredditStatistic(rd));
        }

        return stats;
    }

    private static SubredditStatistic GetSubredditStatistic(SubredditData rd)
    {
        var subredditStatistic = new SubredditStatistic
        {
            Kind = rd.Kind,
            CreateTime = DateTime.Now,
            Author = rd.Data["author"]?.GetValue<string>() ?? string.Empty,
            Id = rd.Data["id"]?.GetValue<string>() ?? string.Empty,
            UpVotes = rd.Data["ups"]?.GetValue<int>() ?? 0,
            Permalink = rd.Data["permalink"]?.GetValue<string>() ?? string.Empty
        };
        return subredditStatistic;
    }
}