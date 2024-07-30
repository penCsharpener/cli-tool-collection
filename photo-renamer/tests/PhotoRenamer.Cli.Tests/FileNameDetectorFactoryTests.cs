using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Tests;

public class FileNameDetectorFactoryTests
{
    private readonly FileNameDetectorFactory _sut;
    private readonly IImageSharpWrapper _imageSharpWrapper;


    public FileNameDetectorFactoryTests()
    {
        _imageSharpWrapper = Substitute.For<ImageSharpWrapper>();

        _sut = new FileNameDetectorFactory(_imageSharpWrapper);
    }

    [Theory]
    [InlineData("IMG_3993.jpg", typeof(CanonImageDetector))]
    [InlineData("HIC_3993.jpg", typeof(NikonFileNameDetector))]
    [InlineData("MVI_3993.jpg", typeof(CanonVideoDetector))]
    [InlineData("IMG_20240706_172000476 name tag.jpg", typeof(MotorolaImageDetector))]
    [InlineData("IMG_20240706_172000476.jpg", typeof(MotorolaImageDetector))]
    [InlineData("IMG_20240706_171903698_HDR.jpg", typeof(MotorolaImageDetector))]
    [InlineData("IMG_20240706_171903698_HDR name tag.jpg", typeof(MotorolaImageDetector))]
    [InlineData("VID_20240706_172000476.jpg", typeof(MotorolaVideoDetector))]
    [InlineData("VID_20240706_172000476 name tag.jpg", typeof(MotorolaVideoDetector))]
    public void Factory_Finds_Right_Type_For_File(string fileName, Type expected)
    {
        var fileNameRecord = new FileName(fileName);

        var result = _sut.GetDetector(fileNameRecord);

        result.GetType().Should().Be(expected);
    }
}