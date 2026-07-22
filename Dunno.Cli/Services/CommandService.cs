using Dunno.Cli.Models;
using Dunno.Cli.Records;
using Dunno.Cli.Repositories;

namespace Dunno.Cli.Services;

public class CommandService(CommandRepository repository, TagService tagService)
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
        return await repository.GetByTagAsync(tagName);
    }

    public async Task<Command> AddAsync(AddCommandInput input)
    {
        var command = new Command
        {
            Executable = input.Executable, Arguments = input.Arguments, Description = input.Description
        };
        command.Id = await repository.AddAsync(command);
        foreach (var name in input.Tags)
        {
            var tag = await tagService.GetByNameAsync(name) ??
                      new Tag { Id = await tagService.AddAsync(name), Name = name };
            await repository.AddTagToCommandAsync(command.Id, tag.Id);
            command.Tags.Add(tag);
        }

        return command;
    }
}