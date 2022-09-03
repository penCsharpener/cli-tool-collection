using Cocona;

namespace PhotoRenamer.Cli.Models;

public class RenameParameters : ICommandParameterSet
{
    [Option("of", Description = "Does not include fully qualified file path in the rename command")]
    public bool OnlyUseFilename { get; set; }

    [Option("cmd", Description = "Use Rename-Item instead of 'ren'.")]
    public bool PreferCmd { get; set; }

    [Option('x', Description = "Executes the renaming command after printing it. Only supported with powershell.")]
    public bool ExecuteRename { get; set; }
}
