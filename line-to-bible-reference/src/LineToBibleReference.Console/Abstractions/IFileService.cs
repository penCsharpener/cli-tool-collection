using System.Text.RegularExpressions;

namespace LineToBibleReference.Console.Abstractions;
public interface IFileService
{
    IAsyncEnumerable<string> ReadByLineAsync(string filePath, CancellationToken cancellationToken = default);
    Task WriteAllTextToFileAsync(string filePath, string fileContent, CancellationToken cancellationToken = default);
    IEnumerable<FileInfo> GetFilesInDirectory(string path, string searchPattern, Regex? regexFilter = null);
}
