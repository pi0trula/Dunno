using Dunno.Cli.Errors;
using Dunno.Cli.Records;

namespace Dunno.Cli.Tests;

public class AddCommandInputTests
{
    [Fact]
    public void Usage_MatchesExpectedText() =>
        Assert.Equal(
            "Usage: add <executable> [--args \"...\"] [--desc \"...\"] [--tags \"...\"]",
            AddCommandErrors.Usage);

    [Fact]
    public void Parse_NoArguments_ThrowsUsage()
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse(["add"]));

        Assert.Equal(AddCommandErrors.Usage, ex.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_WithoutExecutable_Throws(string executable)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse(["add", executable]));

        Assert.Equal(AddCommandErrors.Usage, ex.Message);
    }

    [Theory]
    [InlineData("--args")]
    [InlineData("--desc")]
    [InlineData("--tags")]
    [InlineData("--foo")]
    public void Parse_ExecutableIsFlag_Throws(string flag)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add",
            flag
        ]));

        Assert.Equal(AddCommandErrors.Usage, ex.Message);
    }

    [Fact]
    public void Parse_FlagInPlaceOfExecutable_WithValue_Throws()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            AddCommandInput.Parse(["add", "--desc", "show status"]));

        Assert.Equal(AddCommandErrors.Usage, ex.Message);
    }

    [Theory]
    [InlineData("--foo")]
    [InlineData("-bar")]
    public void Parse_WithUnknownFlag_Throws(string flag)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add", "git",
            flag
        ]));

        Assert.Equal(AddCommandErrors.UnknownFlag(flag), ex.Message);
    }

    [Theory]
    [InlineData("--args")]
    [InlineData("--desc")]
    [InlineData("--tags")]
    public void Parse_FlagSpecifiedMoreThanOnce_Throws(string flag)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add", "git",
            flag, "foo",
            flag, "bar"
        ]));

        Assert.Equal(AddCommandErrors.DuplicateFlag(flag), ex.Message);
    }

    [Theory]
    [InlineData("--args")]
    [InlineData("--desc")]
    [InlineData("--tags")]
    public void Parse_MissingValueForFlag_Throws(string flag)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add", "git",
            flag
        ]));

        Assert.Equal(AddCommandErrors.MissingValue(flag), ex.Message);
    }

    [Theory]
    [InlineData("--args", "--desc")]
    [InlineData("--args", "--tags")]
    [InlineData("--desc", "--args")]
    [InlineData("--desc", "--tags")]
    [InlineData("--tags", "--args")]
    [InlineData("--tags", "--desc")]
    public void Parse_FlagFollowedByAnotherFlag_ThrowsMissingValue(string flag, string nextFlag)
    {
        var ex = Assert.Throws<ArgumentException>(() => AddCommandInput.Parse([
            "add", "git",
            flag,
            nextFlag, "foo"
        ]));

        Assert.Equal(AddCommandErrors.MissingValue(flag), ex.Message);
    }

    [Theory]
    [InlineData("--args", "status", "status", "", new string[0])]
    [InlineData("--desc", "show status", "", "show status", new string[0])]
    [InlineData("--tags", "git, vcs", "", "", new[] { "git", "vcs" })]
    public void Parse_WithOneFlag_ReturnsFilledInput(
        string flag, string value, string expectedArgs, string expectedDesc, string[] expectedTags)
    {
        var input = AddCommandInput.Parse(["add", "git", flag, value]);

        Assert.Equal("git", input.Executable);
        Assert.Equal(expectedArgs, input.Arguments);
        Assert.Equal(expectedDesc, input.Description);
        Assert.Equal(expectedTags, input.Tags);
    }

    [Fact]
    public void Parse_OnlyExecutable_ReturnsEmptyDefaults()
    {
        var input = AddCommandInput.Parse(["add", "git"]);

        Assert.Equal("git", input.Executable);
        Assert.Equal("", input.Arguments);
        Assert.Equal("", input.Description);
        Assert.Empty(input.Tags);
    }

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

    [Theory]
    [InlineData("a, b ,c", new[] { "a", "b", "c" })]
    [InlineData("a,,b", new[] { "a", "b" })]
    [InlineData(",a,", new[] { "a" })]
    [InlineData("a,b,a", new[] { "a", "b" })]
    [InlineData("a,A", new[] { "a" })]
    [InlineData(" , ", new string[0])]
    [InlineData("", new string[0])]
    public void Parse_Tags_AreTrimmedAndDistinct(string value, string[] expected)
    {
        var input = AddCommandInput.Parse(["add", "git", "--tags", value]);

        Assert.Equal(expected, input.Tags);
    }
}