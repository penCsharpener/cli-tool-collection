using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class FileNameStrategyFactoryTests
{
    private readonly FileNameStrategyFactory _sut;
    private readonly IImageSharpWrapper _imageSharpWrapper;


    public FileNameStrategyFactoryTests()
    {
        _imageSharpWrapper = Substitute.For<ImageSharpWrapper>();

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
    public void Factory_Finds_Right_Type_For_File(string fileName, Type expected)
    {
        var fileNameRecord = new FileName(fileName);

        var result = _sut.GetStrategy(fileNameRecord);

        result.GetType().Should().Be(expected);
    }
}