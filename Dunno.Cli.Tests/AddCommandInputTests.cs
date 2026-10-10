using Dunno.Cli.Records;

namespace Dunno.Cli.Tests;

public class AddCommandInputTests
{
    [Fact]
    public void Parse_WithAllFlags_ReturnsFilledInput()
    {
        string[] args = ["add", "git", "--args", "status", "--desc", "show status", "--tags", "git, vcs"];

        var input = AddCommandInput.Parse(args);

        Assert.Equal("git", input.Executable);
        Assert.Equal("status", input.Arguments);
        Assert.Equal("show status", input.Description);
        Assert.Equal(["git", "vcs"], input.Tags);
    }

    [Fact]
    public void Parse_WithoutExecutable_Throws()
    {
        Assert.Throws<ArgumentException>(() => AddCommandInput.Parse(["add"]));
    }

    [Theory]
    [InlineData("--foo")]
    [InlineData("-a")]
    public void Parse_WithUnknownFlag_Throws(string flag)
    {
        Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add", "git",
            flag, "x"
        ]));
    }
}