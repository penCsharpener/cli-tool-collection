using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class PanasonicImageStrategyTests
{
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public PanasonicImageStrategyTests()
    {
        _imageSharpWrapper = Substitute.For<IImageSharpWrapper>();
    }

    [Theory]
    [InlineData("P1010108.jpg", "20240730_111213_P1010108.jpg")]
    [InlineData("P1010108 name tag.jpg", "20240730_111213_P1010108 name tag.jpg")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);
        _imageSharpWrapper.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

        var sut = new PanasonicImageStrategy(fileNameRecord, _imageSharpWrapper);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}

