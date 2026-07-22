using Dapper;
using Dunno.Cli.Models;
using Microsoft.Data.Sqlite;

namespace Dunno.Cli.Repositories;

public class TagRepository(string connectionString)
{
    public async Task<List<Tag>> GetAllAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var tags = await connection.QueryAsync<Tag>("SELECT Id, Name FROM Tags ORDER BY Name");

        return tags.ToList();
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        return await connection.QueryFirstOrDefaultAsync<Tag>(
            "SELECT Id, Name FROM Tags WHERE Name = @Name",
            new { Name = name });
    }

    public async Task<int> AddAsync(string name)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var id = await connection.ExecuteScalarAsync<int>(
            """
            INSERT INTO Tags(Name)
            VALUES(@Name);
            SELECT last_insert_rowid()
            """,
            new { Name = name });

        return id;
    }
}