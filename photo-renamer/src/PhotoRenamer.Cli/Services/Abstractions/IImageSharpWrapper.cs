namespace PhotoRenamer.Cli.Services;

public interface IImageSharpWrapper
{
    public Task<DateTime?> GetCreationDate(string fileName, CancellationToken token);
}
