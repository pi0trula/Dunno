namespace Dunno.Cli.Models;

public class Command
{
    public int Id { get; set; }

    public string Executable { get; set; } = "";
    
    public List<string> Arguments { get; set; } = [];
    
    public string Description { get; set; } = "";
    
    public DateTime CreatedAt { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];
    
    public string FullCommand =>
        $"{Executable} {string.Join(" ", Arguments)}";
}