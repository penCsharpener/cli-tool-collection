using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class CanonVideoStrategyTests
{
    private readonly IImageMetadataWrapper _imageMetadataWrapper;

    public CanonVideoStrategyTests()
    {
        _imageMetadataWrapper = Substitute.For<IImageMetadataWrapper>();
    }

    [Theory]
    [InlineData("MVI_3993.mp4", "16010101_010000 MVI_3993.mp4")]
    [InlineData("MVI_3993 name tag.mp4", "16010101_010000 MVI_3993 name tag.mp4")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);
        _imageMetadataWrapper.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

        var sut = new CanonVideoStrategy(fileNameRecord);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}
