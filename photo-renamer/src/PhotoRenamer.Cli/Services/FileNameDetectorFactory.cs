using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public sealed class FileNameDetectorFactory : IFileNameDetectorFactory
{
    private static readonly Regex _canonImage = new("IMG_\\d{4}.*");
    private static readonly Regex _canonVideo = new("MVI_\\d{4}.*");
    private static readonly Regex _nikonImage = new("HIC_\\d{4}.*");
    private static readonly Regex _motorolaImage = new("IMG_\\d{8}_\\d{9}.*");
    private static readonly Regex _motorolaVideo = new("VID_\\d{8}_\\d{9}.*");
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public FileNameDetectorFactory(IImageSharpWrapper imageSharpWrapper)
    {
        _imageSharpWrapper = imageSharpWrapper;
    }

    public IFileNameDetector GetDetector(FileName fileName)
    {
        if (_motorolaImage.IsMatch(fileName.Name))
        {
            return new MotorolaImageDetector(fileName);
        }

        if (_motorolaVideo.IsMatch(fileName.Name))
        {
            return new MotorolaVideoDetector(fileName);
        }

        if (_canonImage.IsMatch(fileName.Name))
        {
            return new CanonImageDetector(fileName, _imageSharpWrapper);
        }

        if (_canonVideo.IsMatch(fileName.Name))
        {
            return new CanonVideoDetector(fileName);
        }

        if (_nikonImage.IsMatch(fileName.Name))
        {
            return new NikonFileNameDetector(fileName, _imageSharpWrapper);
        }

        throw new NotImplementedException("This file name pattern is not recognized yet.");
    }
}
