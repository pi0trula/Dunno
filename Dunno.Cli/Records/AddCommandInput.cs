namespace Dunno.Cli.Records;

public record AddCommandInput(
    string Executable,
    string Arguments,
    string Description,
    List<string> Tags
)
{
    public static AddCommandInput Parse(string[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException(
                "Usage: add <executable> [--args \"...\"] [--desc \"...\"] [--tags \"...\"]"
            );

        var executable = args[1];

        var arguments = "";
        var description = "";
        var tags = new List<string>();

        var seenFlags = new HashSet<string>();

        for (var i = 2; i < args.Length; i++)
        {
            if (args[i].StartsWith("--") && !seenFlags.Add(args[i]))
                throw new ArgumentException($"Flag specified more than once: {args[i]}");

            switch (args[i])
            {
                case "--args" when i + 1 < args.Length:
                    arguments = args[++i];
                    break;
                case "--desc" when i + 1 < args.Length:
                    description = args[++i];
                    break;
                case "--tags" when i + 1 < args.Length:
                    tags = args[++i]
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Distinct()
                        .ToList();
                    break;
                default:
                    throw new ArgumentException($"Unknown flag: {args[i]}. Allowed: --args, --desc, --tags");
            }
        }

        if (string.IsNullOrWhiteSpace(executable))
            throw new ArgumentException(
                "Usage: add <executable> [--args \"...\"] [--desc \"...\"] [--tags \"...\"]"
            );

        return new AddCommandInput(executable, arguments, description, tags);
    }
}