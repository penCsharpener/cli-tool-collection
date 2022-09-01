namespace PhotoRename.Common.Services.Abstractions;

public interface IFileService
{
    IEnumerable<string> GetFiles(string sDir, params string[]? excludeDirs);
}
