using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Services;

public sealed class FileNameStrategyFactory : IFileNameStrategyFactory
{
    private static readonly Regex _canonImage = new("^IMG_\\d{4}.*");
    private static readonly Regex _panasonicImage = new("^P\\d{7}.*");
    private static readonly Regex _canonVideo = new("^MVI_\\d{4}.*");
    private static readonly Regex _nikonImage = new("^HIC_\\d{4}.*");
    private static readonly Regex _motorolaImage = new("^IMG_\\d{8}_\\d{6,9}.*");
    private static readonly Regex _motorolaVideo = new("^VID_\\d{8}_\\d{6,9}.*");
    private static readonly Regex _samsungFile = new("^\\d{8}_\\d{6}.*");
    private static readonly Regex _removeMillisecondsFile = new("^\\d{8}_\\d{9}.*");
    private static readonly Regex _defaultFile = new("^\\d{8}_\\d{6}.*");
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public FileNameStrategyFactory(IImageSharpWrapper imageSharpWrapper)
    {
        _imageSharpWrapper = imageSharpWrapper;
    }

    public IFileNameStrategy GetStrategy(FileName fileName)
    {
        if (_motorolaImage.IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new MotorolaImageStrategy(fileName);
        }

        if (_motorolaVideo.IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
        {
            return new MotorolaVideoStrategy(fileName);
        }

        if (_canonImage.IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new CanonImageStrategy(fileName, _imageSharpWrapper);
        }

        if (_panasonicImage.IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new PanasonicImageStrategy(fileName, _imageSharpWrapper);
        }

        if (_canonVideo.IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
        {
            return new CanonVideoStrategy(fileName);
        }

        if (_nikonImage.IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new NikonFileNameStrategy(fileName, _imageSharpWrapper);
        }

        if (_removeMillisecondsFile.IsMatch(fileName.Name))
        {
            return new MillisecondsNameStrategy(fileName);
        }

        if (_samsungFile.IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new DefaultNameStrategy(fileName);
        }

        if (_samsungFile.IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
        {
            return new DefaultNameStrategy(fileName);
        }

        return new DefaultNameStrategy(fileName);
    }

    private bool IsImage(string fileExtension)
    {
        var imgExtensions = new[] { ".jpg", ".webp" };

        return imgExtensions.Any(x => fileExtension.Equals(x, StringComparison.OrdinalIgnoreCase));
    }

    private bool IsVideo(string fileExtension)
    {
        var videoExtensions = new[] { ".mp4", ".mov" };

        return videoExtensions.Any(x => fileExtension.Equals(x, StringComparison.OrdinalIgnoreCase));
    }
}
