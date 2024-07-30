using PhotoRename.Common.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IFileNameDetector
{
    Task<RenamePair?> GetRenamePair(CancellationToken token);
}
