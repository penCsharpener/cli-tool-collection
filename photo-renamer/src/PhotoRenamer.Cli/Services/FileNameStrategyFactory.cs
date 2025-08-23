using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Services;

public sealed partial class FileNameStrategyFactory : IFileNameStrategyFactory
{
    private readonly RenameParameters _options;
    private readonly IImageSharpWrapper _imageSharpWrapper;
    private static Regex? _customRegex = null!;

    public FileNameStrategyFactory(IImageSharpWrapper imageSharpWrapper)
    {
        _imageSharpWrapper = imageSharpWrapper;
    }

    public IFileNameStrategy GetStrategy(FileName fileName, string? customRegex)
    {
        if (!string.IsNullOrWhiteSpace(customRegex))
        {
            _customRegex ??= new Regex(customRegex,  RegexOptions.IgnoreCase);

            if (_customRegex.IsMatch($"{fileName.Name}{fileName.FileExtension}"))
            {
                return new CustomRegexFormattingStrategy(fileName, customRegex, _imageSharpWrapper);
            }
        }

        if (MotorolaImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new MotorolaImageStrategy(fileName);
        }

        if (MotorolaVideoRegex().IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
        {
            return new MotorolaVideoStrategy(fileName);
        }

        if (CanonImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new CanonImageStrategy(fileName, _imageSharpWrapper);
        }

        if (PanasonicImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new PanasonicImageStrategy(fileName, _imageSharpWrapper);
        }

        if (CanonVideoRegex().IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
        {
            return new CanonVideoStrategy(fileName);
        }

        if (NikonImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new NikonFileNameStrategy(fileName, _imageSharpWrapper);
        }

        if (CimgImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new CimgImageStrategy(fileName);
        }

        if (PicImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new PicImageStrategy(fileName, _imageSharpWrapper);
        }

        if (FujifilmImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new FujifilmImageStrategy(fileName, _imageSharpWrapper);
        }

        if (SonyImageRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new SonyImageStrategy(fileName, _imageSharpWrapper);
        }

        if (RemoveMillisecondsFileRegex().IsMatch(fileName.Name))
        {
            return new MillisecondsNameStrategy(fileName);
        }

        if (SamsungFileRegex().IsMatch(fileName.Name) && IsImage(fileName.FileExtension))
        {
            return new DefaultNameStrategy(fileName);
        }

        if (SamsungFileRegex().IsMatch(fileName.Name) && IsVideo(fileName.FileExtension))
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
        var videoExtensions = new[] { ".mp4", ".mov", ".avi" };

        return videoExtensions.Any(x => fileExtension.Equals(x, StringComparison.OrdinalIgnoreCase));
    }

    [GeneratedRegex("^IMG_\\d{4}.*")]
    private static partial Regex CanonImageRegex();
    [GeneratedRegex("^P\\d{7}.*")]
    private static partial Regex PanasonicImageRegex();
    [GeneratedRegex("^MVI_\\d{4}.*")]
    private static partial Regex CanonVideoRegex();
    [GeneratedRegex("^HIC_\\d{4}.*")]
    private static partial Regex NikonImageRegex();
    [GeneratedRegex("^CIMG\\d{4}.*")]
    private static partial Regex CimgImageRegex();
    [GeneratedRegex("^DSCF\\d{4}.*")]
    private static partial Regex FujifilmImageRegex();
    [GeneratedRegex("^DSC\\d{5}.*")]
    private static partial Regex SonyImageRegex();
    [GeneratedRegex("^PIC_\\d{4}.*")]
    private static partial Regex PicImageRegex();
    [GeneratedRegex(@"^IMG_\d{8}_\d{6,9}.*")]
    private static partial Regex MotorolaImageRegex();
    [GeneratedRegex(@"^VID_\d{8}_\d{6,9}.*")]
    private static partial Regex MotorolaVideoRegex();
    [GeneratedRegex(@"^\d{8}_\d{6}.*")]
    private static partial Regex SamsungFileRegex();
    [GeneratedRegex(@"^\d{8}_\d{9}.*")]
    private static partial Regex RemoveMillisecondsFileRegex();
    [GeneratedRegex(@"^\d{8}_\d{6}.*")]
    private static partial Regex DefaultRegex();
}
