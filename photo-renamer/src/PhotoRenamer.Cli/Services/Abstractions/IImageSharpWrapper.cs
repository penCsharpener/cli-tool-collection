namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IImageSharpWrapper
{
    public Task<DateTime?> GetCreationDate(string fileName, CancellationToken token);
}
