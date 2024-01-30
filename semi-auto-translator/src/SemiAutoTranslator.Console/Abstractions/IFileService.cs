namespace SemiAutoTranslator.Console.Abstractions;

public interface IFileService
{
    Task AppendToFile(string fileName, string text, CancellationToken token = default);
    public IAsyncEnumerable<string> GetLinesInFile(CancellationToken token = default);
}
