namespace Dunno.Cli.Models;

public class Tag
{
    public int Id { get; set; }
    
    public string Name { get; set; } = "";

    public ICollection<Command> Commands { get; set; } = [];
}