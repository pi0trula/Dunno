using Dunno.Cli.Errors;

namespace Dunno.Cli.Records;

public record AddCommandInput(
    string Executable,
    string Arguments,
    string Description,
    List<string> Tags
)
{
    private const string ArgsFlag = "--args";
    private const string DescFlag = "--desc";
    private const string TagsFlag = "--tags";

    public static readonly IReadOnlyList<string> KnownFlags = [ArgsFlag, DescFlag, TagsFlag];

    public static AddCommandInput Parse(string[] args)
    {
        if (args.Length < 2 || string.IsNullOrWhiteSpace(args[1]) || args[1].StartsWith('-'))
            throw new ArgumentException(AddCommandErrors.Usage);

        var executable = args[1];

        var arguments = "";
        var description = "";
        var tags = new List<string>();

        var seenFlags = new HashSet<string>();

        for (var i = 2; i < args.Length; i++)
        {
            var flag = args[i];

            if (!KnownFlags.Contains(flag))
                throw new ArgumentException(AddCommandErrors.UnknownFlag(flag));

            if (!seenFlags.Add(flag))
                throw new ArgumentException(AddCommandErrors.DuplicateFlag(flag));

            if (i + 1 >= args.Length || KnownFlags.Contains(args[i + 1]))
                throw new ArgumentException(AddCommandErrors.MissingValue(flag));

            var value = args[++i];

            switch (flag)
            {
                case ArgsFlag:
                    arguments = value;
                    break;
                case DescFlag:
                    description = value;
                    break;
                case TagsFlag:
                    tags = value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim().ToLowerInvariant())
                        .Where(t => t.Length > 0)
                        .Distinct()
                        .ToList();
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Flag '{flag}' is listed in {nameof(KnownFlags)} but has no handler.");
            }
        }

        return new AddCommandInput(executable, arguments, description, tags);
    }
}