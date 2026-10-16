using Dunno.Cli.Services;

namespace Dunno.Cli.Commands;

public class ListCommand(CommandService service)
{
    public async Task<int> ExecuteAsync(string[] args)
    {
        var filter = args.Skip(1).ToArray();

        var commands = filter switch
        {
            [] => await service.GetAllAsync(),
            ["tag", var tag] => await service.GetByTagAsync(tag),
            ["tag"] => null,
            [var executable] => await service.GetByExecutableAsync(executable),
            _ => null
        };

        if (commands is null)
        {
            Console.WriteLine($"Invalid arguments: list {string.Join(" ", filter)}");
            Console.WriteLine("Usage: list | list <executable> | list tag <name>");
            return 2;
        }

        if (commands.Count == 0)
        {
            Console.WriteLine("No commands found.");
            return 0;
        }

        foreach (var command in commands)
        {
            Console.WriteLine(
                $"{command.Id}. {command.FullCommand}"
            );
            Console.WriteLine(
                $"   {command.Description}"
            );
            Console.WriteLine(
                $"   {string.Join(" ", command.Tags.Select(t => $"#{t.Name}"))}"
            );
            Console.WriteLine();
        }

        return 0;
    }
}