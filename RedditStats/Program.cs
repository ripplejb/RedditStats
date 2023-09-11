using Microsoft.Extensions.DependencyInjection;
using RedditStats;

var host = Startup.AppStartup();

var app = ActivatorUtilities.CreateInstance<App>(host.Services);

var cancellationTokenSource = new CancellationTokenSource();

var token = cancellationTokenSource.Token;

app.Run(token);

while (!File.Exists("cancel.txt"))
{
    Thread.Sleep(5000);
}
Console.WriteLine("User cancelled...");
cancellationTokenSource.Cancel();
cancellationTokenSource.Dispose();
Console.WriteLine("Shutting Down...");
Thread.Sleep(10000);


