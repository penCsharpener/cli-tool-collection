namespace PhotoRenamer.Cli.Services.Abstractions;

public record WebpConversionResult(string WebpPath, bool AlreadyExisted, bool IsValid);

public interface IWebpConverter
{
    /// <summary>
    /// Converts the image to a lossy WebP file next to the source. The source file is kept.
    /// If the WebP already exists it is not overwritten, but checked: <see cref="WebpConversionResult.IsValid"/>
    /// tells whether it is a readable image with the same dimensions as the source.
    /// </summary>
    WebpConversionResult Convert(string sourcePath, int quality);
}
