using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class ImageMetadataWrapper : IImageMetadataWrapper
{
    public Task<DateTime?> GetCreationDate(string fileName, CancellationToken token)
    {
        var directories = ImageMetadataReader.ReadMetadata(fileName);
        var exif = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

        if (exif is null || !exif.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out var creationDate))
        {
            return Task.FromResult<DateTime?>(null); // TODO: handle null properly
        }

        return Task.FromResult<DateTime?>(creationDate);
    }
}
