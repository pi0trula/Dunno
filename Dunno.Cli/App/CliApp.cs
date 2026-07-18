using Dunno.Cli.Commands;

namespace Dunno.Cli.App;

public class CliApp(ListCommand listCommand, TagCommand tagCommand)
{
    public async Task<int> RunAsync(string[] args)
    {
        if (args.Length != 0)
            return args[0] switch
            {
                "list" => await HandleListAsync(args),
                "tag" => await tagCommand.ExecuteAsync(),
                _ => UnknownCommand(args[0])
            };
        
        PrintHelp();
        return 1;
    }

    private async Task<int> HandleListAsync(string[] args)
    {
        return args.Length switch
        {
            1 => await listCommand.ExecuteAsync(),
            2 => await listCommand.ExecuteAsync(executable: args[1]),
            3 when args[1] == "tag" => await listCommand.ExecuteAsync(tag: args[2]),
            _ => UnknownCommand($"list {string.Join(" ", args.Skip(1))}")
        };
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("  list                Show all commands");
        Console.WriteLine("  list <executable>   Filter by executable (e.g. list docker)");
        Console.WriteLine("  list tag <name>     Filter by tag (e.g. list tag git)");
        Console.WriteLine("  tag                 Show all tags");
    }

    private static int UnknownCommand(string command)
    {
        Console.WriteLine($"Unknown command: {command}");
        PrintHelp();
        return 1;
    }
}