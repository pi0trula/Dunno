using Dapper;
using Dunno.Cli.Models;
using Microsoft.Data.Sqlite;

namespace Dunno.Cli.Repositories;

public class CommandRepository(string connectionString)
{
    public async Task<List<Command>> GetAllAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var commandDictionary = new Dictionary<int, Command>();

        await connection.QueryAsync<Command, Tag, Command>(
            """
            SELECT c.*, t.*
            FROM Commands c
            LEFT JOIN CommandTags ct ON c.Id = ct.CommandId
            LEFT JOIN Tags t ON ct.TagId = t.Id
            """,
            (command, tag) =>
            {
                if (!commandDictionary.TryGetValue(command.Id, out var existingCommand))
                {
                    existingCommand = command;
                    commandDictionary[command.Id] = existingCommand;
                }

                if (tag is { Id: > 0 }) existingCommand.Tags.Add(tag);

                return existingCommand;
            },
            splitOn: "Id"
        );

        return commandDictionary.Values.ToList();
    }

    public async Task<List<Command>> GetByExecutableAsync(string executable)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var commandDictionary = new Dictionary<int, Command>();

        await connection.QueryAsync<Command, Tag, Command>(
            """
            SELECT c.*, t.*
            FROM Commands c
            LEFT JOIN CommandTags ct ON c.Id = ct.CommandId
            LEFT JOIN Tags t ON ct.TagId = t.Id
            WHERE c.Executable = @Executable
            """,
            (command, tag) =>
            {
                if (!commandDictionary.TryGetValue(command.Id, out var existingCommand))
                {
                    existingCommand = command;
                    commandDictionary[command.Id] = existingCommand;
                }

                if (tag is { Id: > 0 }) existingCommand.Tags.Add(tag);

                return existingCommand;
            },
            new { Executable = executable },
            splitOn: "Id"
        );

        return commandDictionary.Values.ToList();
    }

    public async Task<List<Command>> GetByTagAsync(string tagName)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var commandDictionary = new Dictionary<int, Command>();

        await connection.QueryAsync<Command, Tag, Command>(
            """
            SELECT c.*, t.*
            FROM Commands c
            LEFT JOIN CommandTags ct ON c.Id = ct.CommandId
            LEFT JOIN Tags t ON ct.TagId = t.Id
            WHERE c.Id IN (
                SELECT ct2.CommandId
                FROM CommandTags ct2
                JOIN Tags t2 ON ct2.TagId = t2.Id
                WHERE t2.Name = @TagName
            )
            """,
            (command, tag) =>
            {
                if (!commandDictionary.TryGetValue(command.Id, out var existingCommand))
                {
                    existingCommand = command;
                    commandDictionary[command.Id] = existingCommand;
                }

                if (tag is { Id: > 0 }) existingCommand.Tags.Add(tag);

                return existingCommand;
            },
            new { TagName = tagName },
            splitOn: "Id"
        );

        return commandDictionary.Values.ToList();
    }

    public async Task<Command> AddAsync(Command command, IEnumerable<string> tagNames)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        command.Id = await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO Commands(Executable, Arguments, Description, CreatedAt)
            VALUES(@Executable, @Arguments, @Description, datetime('now'));
            SELECT last_insert_rowid()
            """,
            new { command.Executable, command.Arguments, command.Description },
            transaction);

        foreach (var name in tagNames)
        {
            var tagId = await connection.ExecuteScalarAsync<int>(
                """
                INSERT INTO Tags(Name)
                SELECT @Name WHERE NOT EXISTS (SELECT 1 FROM Tags WHERE Name = @Name);
                SELECT Id FROM Tags WHERE Name = @Name
                """,
                new { Name = name },
                transaction);

            await connection.ExecuteAsync(
                """
                INSERT OR IGNORE INTO CommandTags(CommandId, TagId)
                VALUES(@CommandId, @TagId)
                """,
                new { CommandId = command.Id, TagId = tagId },
                transaction);

            command.Tags.Add(new Tag { Id = tagId, Name = name });
        }

        await transaction.CommitAsync();
        return command;
    }

    public async Task AddTagToCommandAsync(int commandId, int tagId)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteScalarAsync<int>(
            """
            INSERT OR IGNORE INTO CommandTags(CommandId, TagId)
            VALUES(@CommandId, @TagId);
            """,
            new { CommandId = commandId, TagId = tagId }
        );
    }
}