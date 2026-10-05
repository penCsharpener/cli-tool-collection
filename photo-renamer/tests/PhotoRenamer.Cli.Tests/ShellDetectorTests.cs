using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services;

namespace PhotoRenamer.Cli.Tests;

public class ShellDetectorTests
{
    [Theory]
    [InlineData(false, null, null, ShellType.Bash)]
    [InlineData(true, null, null, ShellType.PowerShell)]
    [InlineData(true, "/usr/bin/bash", null, ShellType.Bash)]
    [InlineData(true, null, "MINGW64", ShellType.Bash)]
    [InlineData(true, "C:\\Program Files\\PowerShell\\7\\pwsh.exe", null, ShellType.PowerShell)]
    public void Resolve_Detects_Shell(bool isWindows, string? shell, string? msystem, ShellType expected)
    {
        ShellDetector.Resolve(new RenameParameters(), isWindows, shell, msystem).Should().Be(expected);
    }

    [Fact]
    public void Resolve_Explicit_Flags_Win()
    {
        ShellDetector.Resolve(new RenameParameters { PreferBash = true }, true, null, null).Should().Be(ShellType.Bash);
        ShellDetector.Resolve(new RenameParameters { PreferCmd = true }, false, null, null).Should().Be(ShellType.Cmd);
    }

    [Fact]
    public void BashRenameCommand_Quotes_Special_Characters()
    {
        var pair = new RenamePair("/tmp/it's a.jpg", "new name.jpg").ApplyOptions(true);

        pair.BashRenameCommand.Should().Be("mv -n -- 'it'\\''s a.jpg' 'new name.jpg'");
    }
}
