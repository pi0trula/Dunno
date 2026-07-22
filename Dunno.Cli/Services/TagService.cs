using Dunno.Cli.Models;
using Dunno.Cli.Repositories;

namespace Dunno.Cli.Services;

public class TagService(TagRepository repository)
{
    public async Task<List<Tag>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await repository.GetByNameAsync(name);
    }

    public async Task<int> AddAsync(string name)
    {
        return await repository.AddAsync(name);
    }
}