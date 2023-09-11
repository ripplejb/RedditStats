using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditStats.Configs;
using RedditStats.Helpers;
using RedditStats.Services;
using Serilog;
using Serilog.Events;

namespace RedditStats;

public static class Startup
{
    public static IHost AppStartup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File("RedditStats.log")
            .CreateLogger();
    
        Log.Logger.Information("Starting Reddit stat collection app at {date}", DateTime.Now);

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddOptions<RedditConfigs>().Bind(configuration.GetSection(nameof(RedditConfigs)));
                services.AddSingleton<IRateLimitHelper, RateLimitHelper>();
                services.AddSingleton<ISubredditStatisticMapper, SubredditStatisticMapper>();
                services.AddSingleton<ICommentService, CommentService>();
                services.AddSingleton<ILinkService, LinkService>();
                services.AddHttpClient<ISubredditTopService, SubredditTopService>();
                services.AddHttpClient<ISubredditCommentService, SubredditCommentService>();
                services.AddSingleton<App>();
            })
            .UseSerilog()
            .Build();

        return host;
    }    
}