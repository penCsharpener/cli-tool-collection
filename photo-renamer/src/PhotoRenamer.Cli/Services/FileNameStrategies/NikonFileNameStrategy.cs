using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class NikonFileNameStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public NikonFileNameStrategy(FileName fileName, IImageSharpWrapper imageSharpWrapper)
    {
        _fileName = fileName;
        _imageSharpWrapper = imageSharpWrapper;
    }

    public async Task<RenamePair> GetRenamePair(CancellationToken token)
    {
        var creationDate = await _imageSharpWrapper.GetCreationDate(_fileName.FullPath, token);

        return new(_fileName.FullName, $"{creationDate:yyyyMMdd_HHmmss} {_fileName.Name}{_fileName.FileExtension}");
    }
}

