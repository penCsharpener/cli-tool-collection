using PhotoRenamer.Cli.Services;
using Directory = System.IO.Directory;

namespace PhotoRenamer.Cli.Tests;

public class OriginalMarkerTests : IDisposable
{
    private readonly string _directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;

    [Fact]
    public void Mark_And_Restore_Round_Trip_Keep_Content_And_Times()
    {
        var path = Path.Combine(_directory, "IMG_1.jpg");
        File.WriteAllText(path, "content");
        var time = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(path, time);

        var marked = OriginalMarker.Mark(path);

        marked.Should().Be(Path.Combine(_directory, "IMG_1__org.jpg"));
        File.Exists(path).Should().BeFalse();
        File.GetLastWriteTimeUtc(marked!).Should().Be(time);
        OriginalMarker.Mark(marked!).Should().BeNull(); // already marked

        OriginalMarker.Restore(marked!).Should().Be(path);
        File.ReadAllText(path).Should().Be("content");
    }

    [Fact]
    public void Mark_And_Restore_Do_Not_Overwrite_Existing_Files()
    {
        var path = Path.Combine(_directory, "a.mp4");
        File.WriteAllText(path, "new");
        File.WriteAllText(Path.Combine(_directory, "a__org.mp4"), "old");

        OriginalMarker.Mark(path).Should().BeNull();
        OriginalMarker.Restore(Path.Combine(_directory, "a__org.mp4")).Should().BeNull();
        File.ReadAllText(Path.Combine(_directory, "a__org.mp4")).Should().Be("old");
    }

    public void Dispose() => Directory.Delete(_directory, true);
}
