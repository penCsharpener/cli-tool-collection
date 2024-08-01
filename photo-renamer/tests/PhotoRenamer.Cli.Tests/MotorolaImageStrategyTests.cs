using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class MotorolaImageStrategyTests
{
    [Theory]
    [InlineData("IMG_20240706_172000476 name tag.jpg", "20240706_172000_IMG name tag.jpg")]
    [InlineData("IMG_20240706_172000476.jpg", "20240706_172000_IMG.jpg")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);

        var sut = new MotorolaImageStrategy(fileNameRecord);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}
