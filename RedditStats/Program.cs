using Microsoft.Extensions.DependencyInjection;
using RedditStats;


var host = Startup.AppStartup();

var app = ActivatorUtilities.CreateInstance<App>(host.Services);

app.Run();


