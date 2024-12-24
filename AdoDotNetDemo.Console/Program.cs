using AdoDotNetDemo.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddTransient<Repository, Repository>();
});
IHost app = builder.Build();
Repository repository = app.Services.GetRequiredService<Repository>();

// Ping
bool pingResult = await repository.Ping();
Console.WriteLine($"Can access database: {pingResult}");
if (!pingResult) return;

// Setup
bool setupResult = await repository.Setup();
Console.WriteLine($"Completed setup: {setupResult}");
if (!setupResult) return;