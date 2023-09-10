using Microsoft.Extensions.Options;
using RedditStats.Configs;

namespace RedditStats;

public class App
{
    private readonly RedditConfigs _redditConfigs;

    public App(IOptions<RedditConfigs> redditConfigOptions)
    {
        _redditConfigs = redditConfigOptions.Value;
    }

    public void Run(string[] args)
    {
        Console.WriteLine(_redditConfigs.Credentials);
    }
}