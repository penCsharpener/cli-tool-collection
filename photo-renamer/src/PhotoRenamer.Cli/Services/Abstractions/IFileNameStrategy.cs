using PhotoRename.Common.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IFileNameStrategy
{
    Task<RenamePair?> GetRenamePair(CancellationToken token);
}
