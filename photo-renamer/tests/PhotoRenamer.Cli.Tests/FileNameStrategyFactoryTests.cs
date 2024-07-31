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
    [InlineData("HIC_3993.jpg", typeof(NikonFileNameStrategy))]
    [InlineData("MVI_3993.jpg", typeof(CanonVideoStrategy))]
    [InlineData("IMG_20240706_172000476 name tag.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_172000476.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_171903698_HDR.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("IMG_20240706_171903698_HDR name tag.jpg", typeof(MotorolaImageStrategy))]
    [InlineData("VID_20240706_172000476.jpg", typeof(MotorolaVideoStrategy))]
    [InlineData("VID_20240706_172000476 name tag.jpg", typeof(MotorolaVideoStrategy))]
    public void Factory_Finds_Right_Type_For_File(string fileName, Type expected)
    {
        var fileNameRecord = new FileName(fileName);

        var result = _sut.GetStrategy(fileNameRecord);

        result.GetType().Should().Be(expected);
    }
}