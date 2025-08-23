using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class SamsungImageNameStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;

    public SamsungImageNameStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair> GetRenamePair(CancellationToken token)
    {
        return Task.FromResult(new RenamePair(_fileName.FullPath, $"{_fileName.Name}{_fileName.FileExtension}"));
    }
}

