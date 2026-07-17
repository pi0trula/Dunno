using Dunno.Cli.Models;

namespace Dunno.Cli.Services;

public class CommandService
{
    private readonly List<Command> _commands =
    [
        new()
        {
            Id = 1,
            Executable = "git",
            Arguments = ["init"],
            Description = "Creates a new git repository",
            CreatedAt = DateTime.Now,
            Tags =
            [
                new Tag { Name = "git" }
            ]
        },

        new()
        {
            Id = 2,
            Executable = "git",
            Arguments = ["add", "."],
            Description = "Stages all files",
            CreatedAt = DateTime.Now,
            Tags =
            [
                new Tag { Name = "git" },
                new Tag { Name = "stage" }
            ]
        }
    ];


    public List<Command> GetAll()
    {
        return _commands;
    }
}