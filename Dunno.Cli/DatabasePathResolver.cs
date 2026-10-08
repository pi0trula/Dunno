namespace Dunno.Cli;

public static class DatabasePathResolver
{
    public static string Resolve()
    {
        var fromEnv = Environment.GetEnvironmentVariable("DUNNO_DB");

        if (!string.IsNullOrWhiteSpace(fromEnv))
            return fromEnv;

        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "dunno");

        Directory.CreateDirectory(dir);

        return Path.Combine(dir, "dunno.db");
    }
}