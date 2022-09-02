using Cocona;
using PhotoRename.Common.Models;

namespace PhotoTimeStampShifter.Cli.Models;

public class RenameParameters : ICommandParameterSet
{
    private bool _onlyUseFilename;
    private bool _preferCmd;

    [Option("of", Description = "Does not include fully qualified file path in the rename command")]
    public bool OnlyUseFilename
    {
        get => _onlyUseFilename; set
        {
            _onlyUseFilename = value;
            GlobalOptions.OnlyUseFilename = _onlyUseFilename;
        }
    }

    [Option("cmd", Description = "Use Rename-Item instead of 'ren'.")]
    public bool PreferCmd
    {
        get => _preferCmd; set
        {
            _preferCmd = value;
            GlobalOptions.PreferCmd = _preferCmd;
        }
    }

    [Option('x', Description = "Executes the renaming command after printing it. Only supported with powershell.")]
    public bool ExecuteRename { get; set; }

    [Option('s', Description = "The time unit used for shifting timestamp. Ex: -s 10s (10 seconds), -s 15m, -s -4h (-4 hours), -s 6d")]
    public string? TimeShiftValue { get; set; }
}
