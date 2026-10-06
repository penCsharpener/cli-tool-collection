using System.Diagnostics.CodeAnalysis;
using PhotoRename.Common.Services.Abstractions;

namespace PhotoRename.Common.Services;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
    public IEnumerable<string> GetFiles(string rootDir, params string[]? excludeDirs)
    {
        return GetFiles(rootDir, true, excludeDirs);
    }

    public IEnumerable<string> GetFiles(string rootDir, bool recursive, string[]? excludeDirs)
    {
        foreach (var f in GetFilesInDir(rootDir))
        {
            yield return f;
        }

        if (!recursive)
        {
            yield break;
        }

        foreach (var d in GetDirs(rootDir).Where(d => excludeDirs == null || !ExcludeFolders(d, excludeDirs)))
        {
            foreach (var f in GetFiles(d, true, excludeDirs))
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
        return excludeDirs.Any(dir.Contains);
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
