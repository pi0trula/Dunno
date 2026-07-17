using Dunno.Cli.Commands;

namespace Dunno.Cli.CLI;

public class CliApp(ListCommand listCommand)
{
    public int Run(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Available commands: list");
            return 1;
        }

        
        return args[0] switch
        {
            "list" => listCommand.Execute(),

            _ => UnknownCommand(args[0])
        };
    }
    
    private static int UnknownCommand(string command)
    {
        Console.WriteLine($"Unknown command: {command}");
        Console.WriteLine("Available commands: list");

        return 1;
    }
}