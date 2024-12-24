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
    
    public async Task<bool> Setup()
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  BEGIN TRANSACTION;

                                  CREATE TABLE IF NOT EXISTS AchievementClass (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Name TEXT NOT NULL,
                                      Unit TEXT NOT NULL
                                  );

                                  CREATE TABLE IF NOT EXISTS Achievement (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      AchievementClassId INTEGER NOT NULL,
                                      CompletedDate TEXT NOT NULL,
                                      Quantity INTEGER NOT NULL,
                                      RecordedDate TEXT NOT NULL,
                                      FOREIGN KEY (AchievementClassId) REFERENCES AchievementClass(Id)
                                  );

                                  COMMIT;
                                  """;
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public async Task<AchievementClass?> InsertAchievementClass(AchievementClass achievementClass)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  INSERT INTO AchievementClass (Name, Unit) VALUES (@Name, @Unit);
                                  SELECT last_insert_rowid() AS Id;
                                  """;
            command.Parameters.AddWithValue("@Name", achievementClass.Name);
            command.Parameters.AddWithValue("@Unit", achievementClass.Unit);
            await connection.OpenAsync();
            object? output = await command.ExecuteScalarAsync();
            if (output is null) return null;
            achievementClass.Id = (long)output;
            return achievementClass;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<Achievement?> InsertAchievement(Achievement achievement)
    {
        try
        {
            await using SqliteConnection connection = new(_connectionString);
            await using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                                  INSERT INTO Achievement (AchievementClassId, CompletedDate, Quantity, RecordedDate)
                                  VALUES (@AchievementClassId, @CompletedDate, @Quantity, datetime('now'));
                                  SELECT last_insert_rowid() AS Id;
                                  """;
            command.Parameters.AddWithValue("@AchievementClassId", achievement.AchievementClass.Id);
            command.Parameters.AddWithValue("@CompletedDate", achievement.CompletedDate);
            command.Parameters.AddWithValue("@Quantity", achievement.Quantity);
            await connection.OpenAsync();
            object? output = await command.ExecuteScalarAsync();
            if (output is null) return null;
            achievement.Id = (long)output;
            return achievement;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}