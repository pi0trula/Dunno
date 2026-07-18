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
}