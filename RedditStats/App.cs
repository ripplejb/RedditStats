using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStats.Configs;

namespace RedditStats;

public class App
{
    private readonly RedditConfigs _redditConfigs;
    private readonly ILogger<App> _logger;

    public App(IOptions<RedditConfigs> redditConfigOptions, ILogger<App> logger)
    {
        _logger = logger;
        _redditConfigs = redditConfigOptions.Value;
    }

    public void Run()
    {
        _logger.LogInformation("Using credentials from {filename}", _redditConfigs.Credentials);
    }
}