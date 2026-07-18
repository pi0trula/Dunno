using Dunno.Cli.Services;

namespace Dunno.Cli.Commands;

public class TagCommand(TagService tagService)
{
    public async Task<int> ExecuteAsync()
    {
        var tags = await tagService.GetAllAsync();

        if (tags.Count == 0)
        {
            Console.WriteLine("No tags found.");
            return 0;
        }

        foreach (var tag in tags) Console.WriteLine($"  #{tag.Name}");

        return 0;
    }
}