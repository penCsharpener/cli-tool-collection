using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class NikonFileNameStrategyTests
{
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public NikonFileNameStrategyTests()
    {
        _imageSharpWrapper = Substitute.For<IImageSharpWrapper>();
    }

    [Theory]
    [InlineData("HIC_3993.jpg", "20240730_111213 HIC_3993.jpg")]
    [InlineData("HIC_3993 name tag.jpg", "20240730_111213 HIC_3993 name tag.jpg")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);
        _imageSharpWrapper.GetCreationDate(Arg.Any<string>(), CancellationToken.None).Returns(new DateTime(2024, 07, 30, 11, 12, 13));

        var sut = new NikonFileNameStrategy(fileNameRecord, _imageSharpWrapper);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}

