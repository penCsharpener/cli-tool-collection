using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class DefaultNameStrategyTests
{
    [Theory]
    [InlineData("20240706_172000 IMG_3993.jpg", "20240706_172000 IMG_3993.jpg")]
    [InlineData("20240706_172000 IMG_3993 name tag.jpg", "20240706_172000 IMG_3993 name tag.jpg")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);

        var sut = new DefaultNameStrategy(fileNameRecord);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}

