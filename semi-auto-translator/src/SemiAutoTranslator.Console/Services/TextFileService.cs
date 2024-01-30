using System.Diagnostics.CodeAnalysis;
using System.Text;
using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;

namespace SemiAutoTranslator.Console.Services;

[ExcludeFromCodeCoverage(Justification = "Has external dependency: file system")]
public class TextFileService : IFileService
{
    private readonly AppSettings _appSettings;
    private const int DefaultBufferSize = 4096 * 4;
    private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

    public TextFileService(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public async IAsyncEnumerable<string> GetLinesInFile(CancellationToken token = default)
    {
        using var stream = new FileStream(_appSettings.TextSource.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string line;

        while ((line = await reader.ReadLineAsync(token)) != null)
        {
            yield return line;
        }
    }

    public async Task AppendToFile(string fileName, string text, CancellationToken token = default)
    {
        var path = Path.GetDirectoryName(_appSettings.TextSource.FilePath);

        var newFilePath = Path.Combine(path, fileName);

        using var sw = new StreamWriter(newFilePath, true);
        await sw.WriteAsync(text);
    }
}
