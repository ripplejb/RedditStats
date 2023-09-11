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

    public List<SubredditStatistic> GetResult(RedditResponse redditResponse)
    {
        var stats = new List<SubredditStatistic>();
        if (redditResponse.SubredditData.Count == 0)
        {
            _logger.LogWarning("Subreddit data count is 0");
            return stats;
        }
        foreach (var sd in redditResponse.SubredditData)
        {
            if (sd.Kind != "Listing") return stats;

            var children = sd.Data["children"].Deserialize<List<SubredditData>>();
            if (children == null) return stats;
        
            stats.AddRange(from rd in children where rd.Kind == "t3" 
                select _subredditStatisticMapper.MapSubredditData(rd));
        }

        return stats;
    }

}