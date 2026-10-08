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
            var flag = args[i];

            if (flag is not ("--args" or "--desc" or "--tags"))
                throw new ArgumentException($"Unknown flag: {flag}. Allowed: --args, --desc, --tags");

            if (!seenFlags.Add(flag))
                throw new ArgumentException($"Flag specified more than once: {flag}");

            if (i + 1 >= args.Length || args[i + 1] is ("--args" or "--desc" or "--tags"))
                throw new ArgumentException($"Missing value for {flag}.");

            var value = args[++i];

            switch (flag)
            {
                case "--args":
                    arguments = value;
                    break;
                case "--desc":
                    description = value;
                    break;
                case "--tags":
                    tags = value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim())
                        .Where(t => t.Length > 0)
                        .Distinct()
                        .ToList();
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(executable))
            throw new ArgumentException(
                "Usage: add <executable> [--args \"...\"] [--desc \"...\"] [--tags \"...\"]"
            );

        return new AddCommandInput(executable, arguments, description, tags);
    }
}