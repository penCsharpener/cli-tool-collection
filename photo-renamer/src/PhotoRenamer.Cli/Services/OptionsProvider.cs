using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class OptionsProvider : IOptionsProvider
{
    public RenameParameters? Options { get ; set ; }
}
