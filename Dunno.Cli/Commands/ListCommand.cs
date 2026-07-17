using Dunno.Cli.Services;

namespace Dunno.Cli.Commands;

public class ListCommand(CommandService service)
{
    public int Execute()
    {
        var commands = service.GetAll();

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