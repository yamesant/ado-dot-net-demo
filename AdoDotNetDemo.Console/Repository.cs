using Microsoft.Extensions.Hosting;

namespace AdoDotNetDemo.Console;

public sealed class Repository
{
    private readonly string _connectionString;
    public Repository(IHostEnvironment hostEnvironment)
    {
        string dataSource = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdoDotNetDemo",
            hostEnvironment.EnvironmentName,
            "data.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dataSource)!);
        _connectionString = $"Data Source={dataSource}";
    }
}