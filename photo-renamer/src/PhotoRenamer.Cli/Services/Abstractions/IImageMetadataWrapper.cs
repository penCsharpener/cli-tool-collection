namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IImageMetadataWrapper
{
    public Task<DateTime?> GetCreationDate(string fileName, CancellationToken token);
}
