using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class CimgImageStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;

    public CimgImageStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var fi = new FileInfo(_fileName.FullName);
        var creationDate = fi.LastWriteTime;

        return Task.FromResult<RenamePair?>(new RenamePair(_fileName.FullPath, $"{creationDate:yyyyMMdd_HHmmss} {_fileName.Name}{_fileName.FileExtension}"));
    }
}