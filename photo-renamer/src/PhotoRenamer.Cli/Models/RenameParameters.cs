using Cocona;

namespace PhotoRenamer.Cli.Models;

public class RenameParameters : ICommandParameterSet
{
    public RenameParameters() { }
    
    [Option("of", Description = "Does not include fully qualified file path in the rename command")]
    public bool OnlyUseFilename { get; set; }

    [Option("cmd", Description = "Use Rename-Item instead of 'ren'.")]
    public bool PreferCmd { get; set; }

    [Option('x', Description = "Executes the renaming command after printing it. Only supported with powershell.")]
    public bool ExecuteRename { get; set; }

    [Option('v', Description = "Prints version of program.")]
    public bool PrintVersion { get; set; }

    [Option("exif-only", Description = "Only use exif timestamp information.")]
    public bool ExifOnly { get; set; }

    [HasDefaultValue]
    [Option("regex", Description = "Pass in custom regex for other file name formats.")]
    public string? CustomRegex { get; set; } = null!;

    [Option("no-vid", Description = "Exclude videos when renaming.")]
    public bool ExcludeVideos { get; set; }

    [Option('r', Description = "Include subfolders.")]
    public bool Recursive { get; set; }

    [Option('l', Description = "Verbose logging.")]
    public bool VerboseLogging { get; set; }

    [Option("no-err", Description = "Don't log errors.")]
    public bool NoErrorLogging { get; set; }
}
