using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class PanasonicImageStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private readonly IImageMetadataWrapper _imageMetadataWrapper;

    public PanasonicImageStrategy(FileName fileName, IImageMetadataWrapper imageMetadataWrapper)
    {
        _fileName = fileName;
        _imageMetadataWrapper = imageMetadataWrapper;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var creationDate = await _imageMetadataWrapper.GetCreationDate(_fileName.FullPath, token);

        return new(_fileName.FullPath, $"{creationDate:yyyyMMdd_HHmmss}_{_fileName.Name}{_fileName.FileExtension}") { UsedExifTimestamp = true };
    }
}
