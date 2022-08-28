namespace PhotoRenamer.Cli.Services.Abstractions;
public interface IRenameService
{
    IAsyncEnumerable<string> GetNameCommandsAsync(CancellationToken stoppingToken = default);
}
