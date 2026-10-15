using Dunno.Cli.Models;
using Dunno.Cli.Records;
using Dunno.Cli.Repositories;

namespace Dunno.Cli.Services;

public class CommandService(CommandRepository repository)
{
    public async Task<List<Command>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<List<Command>> GetByExecutableAsync(string executable)
    {
        return await repository.GetByExecutableAsync(executable);
    }

    public async Task<List<Command>> GetByTagAsync(string tagName)
    {
        return await repository.GetByTagAsync(tagName.Trim().ToLowerInvariant());
    }

    public async Task<Command> AddAsync(AddCommandInput input)
    {
        var command = new Command
        {
            Executable = input.Executable, Arguments = input.Arguments, Description = input.Description
        };
        return await repository.AddAsync(command, input.Tags);
    }
}