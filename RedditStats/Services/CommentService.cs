using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using RedditStats.Helpers;
using RedditStats.Models;

namespace RedditStats.Services;

public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly ISubredditStatisticMapper _subredditStatisticMapper;
    
    public CommentService(ILogger<CommentService> logger, 
        ISubredditStatisticMapper subredditStatisticMapper)
    {
        _logger = logger;
        _subredditStatisticMapper = subredditStatisticMapper;
    }

    public Tuple<List<string>, List<SubredditStatistic>> GetResult(RedditResponse redditResponse)
    {
        var stats = new List<SubredditStatistic>();
        var uris = new List<string>();
        var stack = new Stack<SubredditData>();

        if (redditResponse.SubredditData.Count == 0)
        {
            _logger.LogWarning("Subreddit data count is 0");
            return new Tuple<List<string>, List<SubredditStatistic>>(uris, stats);
        }

        var permalink = string.Empty;
        foreach (var sd in redditResponse.SubredditData)
        {
            stack.Clear();
            stack.Push(sd);
            while (stack.Count > 0)
            {
                var rd = stack.Pop();
                switch (rd.Kind)
                {
                    case "more":
                    {
                        var children = rd.Data["children"].Deserialize<List<string>>();
                        if (children is null)
                        {
                            continue;
                        }

                        uris.AddRange(children.Select(t => $"{permalink}{t}/.json"));
                        break;
                    }
                    case "t1":
                    {
                        var subredditStatistic = _subredditStatisticMapper.MapSubredditData(rd);
                        stats.Add(subredditStatistic);
                        if (rd.Data["replies"] is JsonValue)
                        {
                            continue;
                        }
                        var replies = rd.Data["replies"].Deserialize<SubredditData>();
                        var children = replies?.Data["children"].Deserialize<List<SubredditData>>();
                        PushToStack(stack, children);
                        break;
                    }
                    case "t3":
                    {
                        // Assuming it always exists
                        permalink = rd.Data["permalink"]?.GetValue<string>() ?? string.Empty;
                        break;
                    }
                    default:
                    {
                        var children = rd.Data["children"].Deserialize<List<SubredditData>>();
                        PushToStack(stack, children);
                        break;
                    }
                }
            }
            
        }
        return new Tuple<List<string>, List<SubredditStatistic>>(uris, stats);
    }

    private static void PushToStack(Stack<SubredditData> subredditDataStack, List<SubredditData>? subredditDataList)
    {
        if (subredditDataList is null)
        {
            return;
        }
        foreach (var t in subredditDataList)
        {
            subredditDataStack.Push(t);
        }
    }
    
}