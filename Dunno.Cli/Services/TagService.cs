using Dunno.Cli.Models;
using Dunno.Cli.Repositories;

namespace Dunno.Cli.Services;

public class TagService(TagRepository repository)
{
    public Task<List<Tag>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }
}