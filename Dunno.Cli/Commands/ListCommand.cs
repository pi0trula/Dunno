using Dunno.Cli.Services;

namespace Dunno.Cli.Commands;

public class ListCommand(CommandService service)
{
    public async Task<int> ExecuteAsync(string? executable = null, string? tag = null)
    {
        var task = (executable, tag) switch
        {
            (not null, _) => service.GetByExecutableAsync(executable),
            (_, not null) => service.GetByTagAsync(tag),
            _             => service.GetAllAsync()
        };

        var commands = await task;

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