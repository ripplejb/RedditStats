using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStats.Configs;
using RedditStats.Models;
using RedditStats.Services;

namespace RedditStats;

public class App
{

    private readonly ICommentService _commentService;
    private readonly ILinkService _linkService;
    private readonly ISubredditTopService _subredditTopService;
    private readonly ISubredditCommentService _subredditCommentService;
    private readonly ILogger<App> _logger;

    public App(ICommentService commentService, ILinkService linkService, 
        ISubredditTopService subredditTopService, ISubredditCommentService subredditCommentService, 
        ILogger<App> logger)
    {
        _commentService = commentService;
        _linkService = linkService;
        _subredditTopService = subredditTopService;
        _subredditCommentService = subredditCommentService;
        _logger = logger;
    }

    /// <summary>
    /// Can be refactored.
    /// </summary>
    /// <param name="token"></param>
    public async Task Run(CancellationToken token)
    {
        var result = new List<SubredditStatistic>();
        var queue = new Queue<string>();

        while (!token.IsCancellationRequested)
        {
            var topResponse = _subredditTopService.Call(10, "week").Result;
            if (topResponse.SubredditData.Count <= 0)
            {
                await Task.Delay(topResponse.RateLimit.ResetSeconds, token);
                continue;
            }
            var remainingCalls = topResponse.RateLimit.RemainingCalls;
            var resetSeconds = topResponse.RateLimit.ResetSeconds;
            var linkResults = _linkService.GetResult(topResponse);
            result.AddRange(linkResults.Item2);
            foreach (var uri in linkResults.Item1) 
            {
                queue.Enqueue(uri);
            }

            while (queue.Count > 0)
            {
                
                var tasks = new List<Task<RedditResponse>>();
                while (remainingCalls > 0 && queue.Count > 0)
                {
                    var uri = queue.Dequeue();
                    remainingCalls--;
                    tasks.Add(Task.Run(() => _subredditCommentService.Call(uri), token));
                }

                topResponse.RateLimit.RemainingCalls = remainingCalls;
                var redditResponses = await Task.WhenAll(tasks);

                foreach (var rs in redditResponses)
                {
                    if (rs.RateLimit.ResetSeconds < resetSeconds)
                    {
                        resetSeconds = rs.RateLimit.ResetSeconds;
                    }
                    if (rs.RateLimit.RemainingCalls < remainingCalls)
                    {
                        remainingCalls = rs.RateLimit.RemainingCalls;
                    }

                    var commentResults = _commentService.GetResult(rs);
                    result.AddRange(commentResults.Item2);
                    foreach (var uri in commentResults.Item1)
                    {
                        queue.Enqueue(uri);
                    }
                }

                if (remainingCalls != 0) continue;
                var topUsers = result.GroupBy(r => r.Author)
                    .Select(r => new { User = r.Key, PostCount = r.Count() })
                    .OrderBy(r => r.PostCount).First();
                var topPost = result.OrderBy(r => r.UpVotes)
                    .Select(r => new { PostId = r.Id, UpVotes = r.UpVotes }).First();
                _logger.LogInformation("Post {post} has upvotes {votes}", topPost.PostId, topPost.UpVotes);
                _logger.LogInformation("User {user} wrote {count} posts.", topUsers.User, topUsers.PostCount);
                await Task.Delay(topResponse.RateLimit.ResetSeconds, token);
            }
        }
    }
}