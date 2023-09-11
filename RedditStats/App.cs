using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStats.Configs;
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

    public void Run()
    {
        
    }
}