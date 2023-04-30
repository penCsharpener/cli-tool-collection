using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;
public class FileService : IFileService
{
    private readonly AppSettings _settings;
    private readonly ILogger<FileService> _logger;

    public FileService(AppSettings settings, ILogger<FileService> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async IAsyncEnumerable<string> ReadByLineAsync(string filePath, [EnumeratorCancellation] CancellationToken token = default)
    {
        if (!File.Exists(filePath))
        {
            yield break;
        }

        using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream, Encoding.UTF8, true, 4096);

        string? line;
        while ((line = await reader.ReadLineAsync(token)) != null)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                yield return line;
            }
        }
    }

    public async Task WriteAllTextToFileAsync(string filePath, string fileContent, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        await File.WriteAllTextAsync(filePath, fileContent, cancellationToken);
    }

    public IEnumerable<FileInfo> GetFilesInDirectory(string path, string searchPattern, Regex? fileRegexFilter = null)
    {
        if (!Directory.Exists(path))
        {
            yield break;
        }

        foreach (var dir in Directory.GetFiles(path, searchPattern, new EnumerationOptions()))
        {
            var file = new FileInfo(dir);

            if (fileRegexFilter is not null && fileRegexFilter.IsMatch(file.Name))
            {
                _logger.LogDebug("{className}: Accessing File {fileName}", nameof(FileService), file.FullName);

                yield return file;
            }
        }
    }

    public async Task WriteCsvAsync<T>(IEnumerable<T> items, string path, string fileName, CancellationToken token = default)
    {
        using var writer = new StreamWriter(Path.Combine(path, fileName));
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(items, token);
    }
}
