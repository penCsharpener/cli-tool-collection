using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;
public interface IRenameService
{
    IAsyncEnumerable<RenamePair> GetNameCommandsAsync(RenameParameters options, CancellationToken stoppingToken = default);
}
