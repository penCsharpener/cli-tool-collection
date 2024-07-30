using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class MotorolaVideoDetector : IFileNameDetector
{
    private readonly FileName _fileName;

    public MotorolaVideoDetector(FileName fileName)
    {
        _fileName = fileName;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var newFileName = string.Concat(_fileName.Name.AsSpan(4, 15), "_VID");
        return new(_fileName.FullName, newFileName + _fileName.FileExtension);
    }
}
