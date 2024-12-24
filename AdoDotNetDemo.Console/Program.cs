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

// Insert achievement class
AchievementClass achievementClass = new() { Name = "Drink water", Unit = "Glass" };
AchievementClass? achievementClassResult = await repository.InsertAchievementClass(achievementClass);
if (achievementClassResult is null)
{
    Console.WriteLine("Failed to insert achievement class");
    return;
}

Console.WriteLine($"Inserted achievement class: {achievementClassResult}");

// Insert achievements
Achievement[] achievements =
[
    new()
    {
        AchievementClass = achievementClassResult,
        CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow),
        Quantity = 2,
    },
    new()
    {
        AchievementClass = achievementClassResult,
        CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow),
        Quantity = 1,
    },
    new()
    {
        AchievementClass = achievementClassResult,
        CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow),
        Quantity = 1,
    },
];
foreach (Achievement achievement in achievements)
{
    Achievement? achievementResult = await repository.InsertAchievement(achievement);
    if (achievementResult is null)
    {
        Console.WriteLine($"Failed to insert achievement {achievement}");
        return;
    }
}
Console.WriteLine("Successfully inserted achievements");