using System.Text;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;
public class FileService : IFileService
{
    private readonly AppSettings _settings;

    public FileService(AppSettings settings)
    {
        _settings = settings;
    }

    public async IAsyncEnumerable<string> ReadByLineAsync()
    {
        using var stream = File.OpenRead(_settings.PathToTextFile);
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 4096);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return line;
        }
    }
}
