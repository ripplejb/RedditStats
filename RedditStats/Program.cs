// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedditStats;
using RedditStats.Configs;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var serviceCollection = new ServiceCollection();

serviceCollection.AddOptions<RedditConfigs>().Bind(configuration.GetSection(nameof(RedditConfigs)));
serviceCollection.AddTransient<App>();

var serviceProvider = serviceCollection.BuildServiceProvider();
serviceProvider.GetService<App>()?.Run(args);
    