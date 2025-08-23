using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class PicImageStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public PicImageStrategy(FileName fileName, IImageSharpWrapper imageSharpWrapper)
    {
        _fileName = fileName;
        _imageSharpWrapper = imageSharpWrapper;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var creationDate = await _imageSharpWrapper.GetCreationDate(_fileName.FullPath, token);
        
        return new(_fileName.FullPath, $"{creationDate:yyyyMMdd_HHmmss} {_fileName.Name}{_fileName.FileExtension}") { UsedExifTimestamp = true };
    }
}