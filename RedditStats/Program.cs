using Microsoft.Extensions.DependencyInjection;
using RedditStats;

var host = Startup.AppStartup();

var app = ActivatorUtilities.CreateInstance<App>(host.Services);

var cancellationTokenSource = new CancellationTokenSource();

var token = cancellationTokenSource.Token;

await app.Run(token);




