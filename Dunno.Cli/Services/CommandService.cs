using Dunno.Cli.Models;
using Dunno.Cli.Repositories;

namespace Dunno.Cli.Services;

public class CommandService(CommandRepository repository)
{
    public Task<List<Command>> GetAllAsync()
    {
        return repository.GetAllAsync();
    }

    public Task<List<Command>> GetByExecutableAsync(string executable)
    {
        return repository.GetByExecutableAsync(executable);
    }

    public Task<List<Command>> GetByTagAsync(string tagName)
    {
        return repository.GetByTagAsync(tagName);
    }
}