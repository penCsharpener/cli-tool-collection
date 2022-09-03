using PhotoRenamer.Cli.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;
public interface IRenameService
{
    IAsyncEnumerable<string> GetNameCommandsAsync(RenameParameters options, CancellationToken stoppingToken = default);
}
