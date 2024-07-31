using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Services;

public sealed class FileNameStrategyFactory : IFileNameStrategyFactory
{
    private static readonly Regex _canonImage = new("IMG_\\d{4}.*");
    private static readonly Regex _canonVideo = new("MVI_\\d{4}.*");
    private static readonly Regex _nikonImage = new("HIC_\\d{4}.*");
    private static readonly Regex _motorolaImage = new("IMG_\\d{8}_\\d{9}.*");
    private static readonly Regex _motorolaVideo = new("VID_\\d{8}_\\d{9}.*");
    private static readonly Regex _samsungFile = new("\\d{8}_\\d{6}.*");
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public FileNameStrategyFactory(IImageSharpWrapper imageSharpWrapper)
    {
        _imageSharpWrapper = imageSharpWrapper;
    }

    public IFileNameStrategy GetStrategy(FileName fileName)
    {
        if (_motorolaImage.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
        {
            return new MotorolaImageStrategy(fileName);
        }

        if (_motorolaVideo.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
        {
            return new MotorolaVideoStrategy(fileName);
        }

        if (_canonImage.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
        {
            return new CanonImageStrategy(fileName, _imageSharpWrapper);
        }

        if (_canonVideo.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
        {
            return new CanonVideoStrategy(fileName);
        }

        if (_nikonImage.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
        {
            return new NikonFileNameStrategy(fileName, _imageSharpWrapper);
        }

        if (_samsungFile.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
        {
            return new DefaultNameStrategy(fileName);
        }

        if (_samsungFile.IsMatch(fileName.Name) && fileName.FileExtension.Equals(".mp4", StringComparison.OrdinalIgnoreCase))
        {
            return new DefaultNameStrategy(fileName);
        }

        return new DefaultNameStrategy(fileName);
    }
}
