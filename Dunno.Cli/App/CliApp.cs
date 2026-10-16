using Dunno.Cli.Commands;

namespace Dunno.Cli.App;

public class CliApp(ListCommand listCommand, AddCommand addCommand, TagCommand tagCommand)
{
    public async Task<int> RunAsync(string[] args)
    {
        if (args.Length != 0)
            return args[0] switch
            {
                "list" => await listCommand.ExecuteAsync(args),
                "add" => await addCommand.ExecuteAsync(args),
                "tag" => await tagCommand.ExecuteAsync(),
                "help" or "-h" or "--help" => PrintHelp(),
                _ => UnknownCommand(args[0])
            };

        PrintHelp();
        return 1;
    }

    private static int PrintHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("  list                  Show all commands");
        Console.WriteLine("  list <executable>     Filter by executable (e.g. list docker)");
        Console.WriteLine("  list tag <name>       Filter by tag (e.g. list tag git)");
        Console.WriteLine("  tag                   Show all tags");
        Console.WriteLine("  add <executable>      Add a command");
        Console.WriteLine("    [--args \"...\"]      Optional: arguments to save");
        Console.WriteLine("    [--desc \"...\"]      Optional: description to save");
        Console.WriteLine("    [--tags \"...\"]      Optional: tags to save");

        return 0;
    }

    private static int UnknownCommand(string command)
    {
        Console.WriteLine($"Unknown command: {command}");
        PrintHelp();
        return 1;
    }
}