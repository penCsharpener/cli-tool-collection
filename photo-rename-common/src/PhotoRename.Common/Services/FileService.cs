using System.Diagnostics.CodeAnalysis;
using PhotoRename.Common.Services.Abstractions;

namespace PhotoRename.Common.Services;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    public IEnumerable<string> GetFiles(string rootDir, params string[]? excludeDirs)
    {
        foreach (var f in GetFilesInDir(rootDir))
        {
            yield return f;
        }

        foreach (var d in GetDirs(rootDir).Where(d => excludeDirs == null || ExcludeFolders(rootDir, excludeDirs)))
        {
            foreach (var f in GetFilesInDir(d))
            {
                yield return f;
            }

            foreach (var f in GetFiles(d, excludeDirs))
            {
                yield return f;
            }
        }
    }

    private static IEnumerable<string> GetFilesInDir(string subDir)
    {
        try
        {
            return Directory.GetFiles(subDir);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    private static bool ExcludeFolders(string dir, string[] excludeDirs)
    {
        return excludeDirs.Any(d => dir.Contains(d));
    }

    private static IEnumerable<string> GetDirs(string subDir)
    {
        try
        {
            return Directory.GetDirectories(subDir);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }
}
