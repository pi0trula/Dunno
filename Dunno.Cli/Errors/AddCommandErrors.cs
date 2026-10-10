using Dunno.Cli.Records;

namespace Dunno.Cli.Errors;

public static class AddCommandErrors
{
    public static readonly string Usage =
        "Usage: add <executable> " + string.Join(" ", AddCommandInput.KnownFlags.Select(f => $"[{f} \"...\"]"));

    public static string UnknownFlag(string flag) =>
        $"Unknown flag: {flag}. Allowed: {string.Join(", ", AddCommandInput.KnownFlags)}";

    public static string DuplicateFlag(string flag) =>
        $"Flag specified more than once: {flag}";

    public static string MissingValue(string flag) =>
        $"Missing value for {flag}.";
}