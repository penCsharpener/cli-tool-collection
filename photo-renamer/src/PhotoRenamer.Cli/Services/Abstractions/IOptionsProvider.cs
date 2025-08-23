using PhotoRenamer.Cli.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IOptionsProvider
{
    public RenameParameters? Options { get; set; }
}