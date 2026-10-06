using Dunno.Cli.Records;
using Dunno.Cli.Services;

namespace Dunno.Cli.Commands;

public class AddCommand(CommandService commandService)
{
    public async Task<int> ExecuteAsync(string[] args)
    {
        try
        {
            var addCommandInput = AddCommandInput.Parse(args);
            var command = await commandService.AddAsync(addCommandInput);

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
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae.Message);
            return 2;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to save command");
            return 1;
        }

        return 0;
    }
}