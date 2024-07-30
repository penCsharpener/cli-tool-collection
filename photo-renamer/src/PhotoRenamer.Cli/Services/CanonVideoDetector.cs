using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class CanonVideoDetector : IFileNameDetector
{
    private readonly FileName _fileName;

    public CanonVideoDetector(FileName fileName)
    {
        _fileName = fileName;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var fi = new FileInfo(_fileName.FullName);
        var creationDate = fi.LastWriteTime;

        return new(_fileName.FullName, $"{creationDate:yyyyMMdd_HHmmss}_{_fileName.Name}{_fileName.FileExtension}");
    }
}
