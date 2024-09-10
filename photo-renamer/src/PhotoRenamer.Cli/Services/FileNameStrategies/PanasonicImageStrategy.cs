using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class PanasonicImageStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public PanasonicImageStrategy(FileName fileName, IImageSharpWrapper imageSharpWrapper)
    {
        _fileName = fileName;
        _imageSharpWrapper = imageSharpWrapper;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var creationDate = await _imageSharpWrapper.GetCreationDate(_fileName.FullPath, token);

        return new(_fileName.FullName, $"{creationDate:yyyyMMdd_HHmmss}_{_fileName.Name}{_fileName.FileExtension}");
    }
}
