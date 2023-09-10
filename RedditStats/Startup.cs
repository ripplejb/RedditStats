using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditStats.Configs;
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
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<RedditConfigs>().Bind(configuration.GetSection(nameof(RedditConfigs)));
                services.AddSingleton<App>();
            })
            .UseSerilog()
            .Build();

        return host;
    }    
}