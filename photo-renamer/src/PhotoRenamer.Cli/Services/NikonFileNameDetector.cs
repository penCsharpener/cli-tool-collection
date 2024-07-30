using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class NikonFileNameDetector : IFileNameDetector
{
    private readonly FileName _fileName;
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public NikonFileNameDetector(FileName fileName, IImageSharpWrapper imageSharpWrapper)
    {
        _fileName = fileName;
        _imageSharpWrapper = imageSharpWrapper;
    }

    public async Task<RenamePair> GetRenamePair(CancellationToken token)
    {
        var creationDate = await _imageSharpWrapper.GetCreationDate(_fileName.Name, token);

        return new(_fileName.FullName, $"{creationDate:yyyyMMdd_HHmmss}_{_fileName.Name}{_fileName.FileExtension}");
    }
}
