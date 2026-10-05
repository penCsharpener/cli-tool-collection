namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IWebpConverter
{
    /// <summary>
    /// Converts the image to a lossy WebP file next to the source and returns the path of the new file,
    /// or null if the target already exists. The source file is kept.
    /// </summary>
    string? Convert(string sourcePath, int quality);
}
