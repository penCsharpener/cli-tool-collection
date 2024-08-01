using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.FileNameStrategies;

namespace PhotoRenamer.Cli.Tests;

public class MotorolaVideoStrategyTests
{
    [Theory]
    [InlineData("VID_20240706_172000476 name tag.mp4", "20240706_172000_VID name tag.mp4")]
    [InlineData("VID_20240706_172000476.mp4", "20240706_172000_VID.mp4")]
    public async Task Strategy_Renames_File(string fileName, string expected)
    {
        var fileNameRecord = new FileName(fileName);

        var sut = new MotorolaVideoStrategy(fileNameRecord);

        var result = await sut.GetRenamePair(CancellationToken.None);

        result.Should().NotBeNull();
        result!.NewFileName.Should().Be(expected);
    }
}
