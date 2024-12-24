using Microsoft.Data.Sqlite;
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
    
    public async Task<bool> Ping()
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await connection.OpenAsync();
            Object? output = await command.ExecuteScalarAsync();
            return output != null && (long)output == 1;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}