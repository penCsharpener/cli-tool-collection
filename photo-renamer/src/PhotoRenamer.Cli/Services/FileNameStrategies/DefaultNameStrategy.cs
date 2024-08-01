using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class DefaultNameStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;


    public DefaultNameStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair> GetRenamePair(CancellationToken token)
    {
        return Task.FromResult(new RenamePair(_fileName.FullName, $"{_fileName.Name}{_fileName.FileExtension}"));
    }
}

