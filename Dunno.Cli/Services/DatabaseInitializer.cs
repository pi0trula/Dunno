using Microsoft.Data.Sqlite;

namespace Dunno.Cli.Services;

public class DatabaseInitializer(string connectionString)
{
    public async Task InitializeAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = """
                                  CREATE TABLE IF NOT EXISTS Commands (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Executable TEXT NOT NULL,
                                      ArgumentsJson TEXT NOT NULL DEFAULT '[]',
                                      Description TEXT NOT NULL DEFAULT '',
                                      CreatedAt TEXT NOT NULL
                                  );
                              """;
            await cmd.ExecuteNonQueryAsync();
        }

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = """
                                  CREATE TABLE IF NOT EXISTS Tags (
                                      Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                      Name TEXT NOT NULL UNIQUE
                                  );
                              """;
            await cmd.ExecuteNonQueryAsync();
        }

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = """
                                  CREATE TABLE IF NOT EXISTS CommandTags (
                                      CommandId INTEGER NOT NULL,
                                      TagId INTEGER NOT NULL,
                                      PRIMARY KEY (CommandId, TagId),
                                      FOREIGN KEY (CommandId) REFERENCES Commands(Id) ON DELETE CASCADE,
                                      FOREIGN KEY (TagId) REFERENCES Tags(Id) ON DELETE CASCADE
                                  );
                              """;
            await cmd.ExecuteNonQueryAsync();
        }
    }
}