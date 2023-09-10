using System.Text.Json;
using RedditStats.Models;

namespace RedditStats.Services;

public class CommentService : ICommentService
{
    public Tuple<List<string>, List<SubredditStatistic>> GetResult(RedditResponse redditResponse)
    {
        var stats = new List<SubredditStatistic>();
        var uris = new List<string>();
        var stack = new Stack<SubredditData>();

        if (redditResponse.SubredditData.Count == 0)
        {
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
                        var subredditStatistic = GetSubredditStatistic(rd);
                        stats.Add(subredditStatistic);
                        if ((rd.Data["replies"]?.GetValue<string>() ?? string.Empty) == string.Empty)
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
    
    private static SubredditStatistic GetSubredditStatistic(SubredditData rd)
    {
        var subredditStatistic = new SubredditStatistic
        {
            Kind = rd.Kind,
            CreateTime = DateTime.Now,
            Author = rd.Data["author"]?.GetValue<string>() ?? string.Empty,
            Id = rd.Data["id"]?.GetValue<string>() ?? string.Empty,
            UpVotes = rd.Data["ups"]?.GetValue<int>() ?? 0
        };
        return subredditStatistic;
    }
}