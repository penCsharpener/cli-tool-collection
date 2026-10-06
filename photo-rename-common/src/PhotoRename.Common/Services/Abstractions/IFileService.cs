namespace PhotoRename.Common.Services.Abstractions;

public interface IFileService
{
    /// <summary>Lists all files below <paramref name="sDir"/>, including subfolders.</summary>
    IEnumerable<string> GetFiles(string sDir, params string[]? excludeDirs);

    /// <summary>Lists the files in <paramref name="sDir"/>; subfolders are only included when <paramref name="recursive"/> is set. Every file is returned once.</summary>
    IEnumerable<string> GetFiles(string sDir, bool recursive, string[]? excludeDirs);
}
