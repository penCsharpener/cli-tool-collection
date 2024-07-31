using PhotoRenamer.Cli.Services.Abstractions;
using SixLabors.ImageSharp;

namespace PhotoRenamer.Cli.Services;

public class ImageSharpWrapper : IImageSharpWrapper
{
    public async Task<DateTime?> GetCreationDate(string fileName, CancellationToken token)
    {
        using var image = await Image.LoadAsync(fileName, token);
        var exif = image.Metadata.ExifProfile;

        if (exif is null)
        {
            return null; // TODO: handle null properly
        }

        if (!exif.TryGetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTimeOriginal, out var creationDateTag))
        {
            return null; // TODO: handle null properly
        }

        if (creationDateTag?.Value is null)
        {
            return null; // TODO: handle null properly
        }

        var creationDateElements = creationDateTag.Value.Split(':', StringSplitOptions.RemoveEmptyEntries);
        var creationDateString = $"{creationDateElements[0]}-{creationDateElements[1]}-{creationDateElements[2]}:{creationDateElements[3]}:{creationDateElements[4]}";
        return DateTime.Parse(creationDateString);
    }
}