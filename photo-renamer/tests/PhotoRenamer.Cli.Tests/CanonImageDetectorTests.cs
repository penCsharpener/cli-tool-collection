using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Tests;

public class CanonImageDetectorTests
{
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public CanonImageDetectorTests()
    {
        _imageSharpWrapper = Substitute.For<IImageSharpWrapper>();
    }

    [Theory]
    [InlineData("IMG_3993.jpg", "20240730_111213_IMG_3993.jpg")]
    [InlineData("IMG_3993 name tag.jpg", "20240730_111213_IMG_3993 name tag.jpg")]
    public async Task Detector_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);
        _imageSharpWrapper.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

        var sut = new CanonImageDetector(fileNameRecord, _imageSharpWrapper);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}
