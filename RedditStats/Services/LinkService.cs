using System.Text.Json;
using Microsoft.Extensions.Logging;
using RedditStats.Helpers;
using RedditStats.Models;

namespace RedditStats.Services;

public class LinkService : ILinkService
{
    
    private readonly ILogger<LinkService> _logger;
    private readonly ISubredditStatisticMapper _subredditStatisticMapper;
    
    public LinkService(ILogger<LinkService> logger, 
        ISubredditStatisticMapper subredditStatisticMapper)
    {
        _logger = logger;
        _subredditStatisticMapper = subredditStatisticMapper;
    }

    public Tuple<List<string>, List<SubredditStatistic>> GetResult(RedditResponse redditResponse)
    {
        var stats = new List<SubredditStatistic>();
        var permalinks = new List<string>();
        if (redditResponse.SubredditData.Count == 0)
        {
            _logger.LogWarning("Subreddit data count is 0");
            return new Tuple<List<string>, List<SubredditStatistic>>(permalinks, stats);
        }
        foreach (var sd in redditResponse.SubredditData)
        {
            if (sd.Kind != "Listing") return new Tuple<List<string>, List<SubredditStatistic>>(permalinks, stats);

            var children = sd.Data["children"].Deserialize<List<SubredditData>>();
            if (children == null) return new Tuple<List<string>, List<SubredditStatistic>>(permalinks, stats);

            foreach (var rd in children.Where(rd => rd.Kind == "t3"))
            {
                stats.Add(_subredditStatisticMapper.MapSubredditData(rd));
                var pl = rd.Data["permalinks"]?.GetValue<string>() ?? string.Empty;
                if (pl == string.Empty) continue;
                permalinks.Add(pl);
            }
        }

        return new Tuple<List<string>, List<SubredditStatistic>>(permalinks, stats);
    }

}