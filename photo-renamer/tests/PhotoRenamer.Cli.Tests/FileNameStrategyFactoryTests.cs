using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class FileNameStrategyFactoryTests
{
    private readonly FileNameStrategyFactory _sut;
    private readonly IImageSharpWrapper _imageSharpWrapper;
    private readonly string _customRegex = "^\\d{3}_\\d{8}";

    public FileNameStrategyFactoryTests()
    {
        _imageSharpWrapper = Substitute.For<IImageSharpWrapper>();

        _sut = new FileNameStrategyFactory(_imageSharpWrapper);
    }

    [Theory]
    [InlineData("IMG_3993.jpg", typeof(CanonImageStrategy))]
    [InlineData("20240706_172000 IMG_3993.jpg", typeof(DefaultNameStrategy))]
    [InlineData("HIC_3993.jpg", typeof(NikonFileNameStrategy))]
    [InlineData("MVI_3993.mp4", typeof(CanonVideoStrategy))]
    [InlineData("20240706_172000 MVI_3993.mp4", typeof(DefaultNameStrategy))]
    [InlineData("20240706_172000476_IMG name tag.jpg", typeof(MillisecondsNameStrategy))]
    [InlineData("20240706_172000476_VID name tag.mp4", typeof(MillisecondsNameStrategy))]
    [InlineData("IMG_20240706_172000476 name tag.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_172000476.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_172000476.webp", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_171903698_HDR.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_171903698_HDR name tag.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("VID_20240706_172000476.mp4", typeof(MotorolaVideoStrategy))]
    [InlineData("VID_20240706_172000476.mov", typeof(MotorolaVideoStrategy))]
    [InlineData("VID_20240706_172000476 name tag.mp4", typeof(MotorolaVideoStrategy))]
    [InlineData("P1010108.JPG", typeof(PanasonicImageStrategy))]
    [InlineData("100_10205030.JPG", typeof(CustomRegexFormattingStrategy))]
    public void Factory_Finds_Right_Type_For_File(string fileName, Type expected)
    {
        var fileNameRecord = new FileName(fileName);

        var result = _sut.GetStrategy(fileNameRecord, _customRegex);

        result.GetType().Should().Be(expected);
    }

    [Theory]
    [InlineData("IMG_3993.jpg", "20240730_111213 IMG_3993.jpg", typeof(CanonImageStrategy))]
    [InlineData("IMG_3993 name tag.jpg", "20240730_111213 IMG_3993 name tag.jpg", typeof(CanonImageStrategy))]
    [InlineData("MVI_3993.mp4", "16010101_010000 MVI_3993.mp4", typeof(CanonVideoStrategy))]
    [InlineData("MVI_3993 name tag.mp4", "16010101_010000 MVI_3993 name tag.mp4", typeof(CanonVideoStrategy))]
    [InlineData("IMG_20240706_172000476 name tag.jpg", "20240706_172000_IMG name tag.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_172000476.jpg", "20240706_172000_IMG.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("20240706_172000 IMG_3993.jpg", "20240706_172000 IMG_3993.jpg", typeof(DefaultNameStrategy))]
    [InlineData("20240706_172000 IMG_3993 name tag.jpg", "20240706_172000 IMG_3993 name tag.jpg", typeof(DefaultNameStrategy))]
    [InlineData("20240706_172000_IMG.jpg", "20240706_172000_IMG.jpg", typeof(DefaultNameStrategy))]
    [InlineData("IMG_20140829_211913.webp", "20140829_211913_IMG.webp", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20140829_211913012.webp", "20140829_211913_IMG.webp", typeof(MotorolaImageStrategy))]
    [InlineData("VID_20240706_172000476 name tag.mp4", "20240706_172000_VID name tag.mp4", typeof(MotorolaVideoStrategy))]
    [InlineData("VID_20240706_172000476.mp4", "20240706_172000_VID.mp4", typeof(MotorolaVideoStrategy))]
    [InlineData("HIC_3993.jpg", "20240730_111213 HIC_3993.jpg", typeof(NikonFileNameStrategy))]
    [InlineData("HIC_3993 name tag.jpg", "20240730_111213 HIC_3993 name tag.jpg", typeof(NikonFileNameStrategy))]
    [InlineData("P1010108.jpg", "20240730_111213_P1010108.jpg", typeof(PanasonicImageStrategy))]
    [InlineData("P1010108 name tag.jpg", "20240730_111213_P1010108 name tag.jpg", typeof(PanasonicImageStrategy))]
    [InlineData("100_10205030.jpg", "20240730_111213 100_10205030.jpg", typeof(CustomRegexFormattingStrategy))]
    public async Task Factory_Transforms_FileName_Correctly(string fileName, string expected, Type expectedStrategy)
    {
        var fileNameRecord = new FileName(fileName);
        _imageSharpWrapper.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

        var strategy = _sut.GetStrategy(fileNameRecord, _customRegex);

        strategy.Should().BeOfType(expectedStrategy);

        var result = await strategy.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}