namespace LineToBibleReference.Console.Abstractions;
public interface IFileService
{
    IAsyncEnumerable<string> ReadByLineAsync(string filePath, CancellationToken cancellationToken = default);
}
