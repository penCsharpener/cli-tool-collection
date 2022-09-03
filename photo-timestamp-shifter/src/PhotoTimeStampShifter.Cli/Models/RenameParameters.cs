using Cocona;

namespace PhotoTimeStampShifter.Cli.Models;

public class RenameParameters : ICommandParameterSet
{
    [Option("of", Description = "Does not include fully qualified file path in the rename command")]
    public bool OnlyUseFilename { get; set; }

    [Option("cmd", Description = "Use Rename-Item instead of 'ren'.")]
    public bool PreferCmd { get; set; }

    [Option('x', Description = "Executes the renaming command after printing it. Only supported with powershell.")]
    public bool ExecuteRename { get; set; }

    [Option('s', Description = "The time unit used for shifting timestamp. Ex: -s 10s (10 seconds), -s 15m, -s -4h (-4 hours), -s 6d, -s 0.04:00:00t (+4 hours)")]
    public string? TimeShiftValue { get; set; }
}
