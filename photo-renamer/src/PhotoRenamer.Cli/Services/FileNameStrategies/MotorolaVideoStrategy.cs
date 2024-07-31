using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class MotorolaVideoStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;

    public MotorolaVideoStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var newFileName = string.Concat(_fileName.Name.AsSpan(4, 15), "_VID");
        return Task.FromResult<RenamePair?>(new(_fileName.FullName, newFileName + _fileName.FileExtension));
    }
}
