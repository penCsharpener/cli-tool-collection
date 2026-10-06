using PhotoRenamer.Cli.Models;

namespace PhotoRenamer.Cli.Services;

public enum ShellType
{
    PowerShell,
    Cmd,
    Bash,
}

public static class ShellDetector
{
    public static ShellType Resolve(RenameParameters options)
    {
        return Resolve(options, OperatingSystem.IsWindows(), Environment.GetEnvironmentVariable("SHELL"), Environment.GetEnvironmentVariable("MSYSTEM"));
    }

    public static ShellType Resolve(RenameParameters options, bool isWindows, string? shellVariable, string? msystemVariable)
    {
        if (options.PreferBash)
        {
            return ShellType.Bash;
        }

        if (options.PreferCmd)
        {
            return ShellType.Cmd;
        }

        // Linux/macOS always use bash-style commands; on Windows, Git Bash/MSYS/Cygwin set SHELL (and MSYSTEM).
        var inBash = !isWindows
            || !string.IsNullOrWhiteSpace(msystemVariable)
            || IsBashLike(shellVariable);

        return inBash ? ShellType.Bash : ShellType.PowerShell;
    }

    public static string ChangeDirectoryCommand(ShellType shell, string directory)
    {
        return shell switch
        {
            ShellType.Bash => $"cd -- '{directory.Replace('\\', '/').Replace("'", "'\\''")}'",
            ShellType.Cmd => $"cd /d \"{directory}\"",
            _ => $"Set-Location -LiteralPath '{directory.Replace("'", "''")}'",
        };
    }

    private static bool IsBashLike(string? shellVariable)
    {
        var shellName = Path.GetFileNameWithoutExtension(shellVariable?.Replace('\\', '/') ?? string.Empty);

        return shellName is "bash" or "sh" or "zsh" or "dash";
    }
}
