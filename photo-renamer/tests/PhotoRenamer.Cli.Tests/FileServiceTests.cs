using PhotoRename.Common.Services;
using Directory = System.IO.Directory;

namespace PhotoRenamer.Cli.Tests;

public class FileServiceTests : IDisposable
{
    private readonly string _root = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;

    public FileServiceTests()
    {
        // the root itself has no files, only subfolders
        Directory.CreateDirectory(Path.Combine(_root, "a", "b"));
        File.WriteAllText(Path.Combine(_root, "a", "one.jpg"), "x");
        File.WriteAllText(Path.Combine(_root, "a", "b", "two.jpg"), "x");
    }

    [Fact]
    public void GetFiles_Recursive_Returns_Every_File_In_Subfolders_Exactly_Once()
    {
        var files = new FileService().GetFiles(_root, true, null).Select(f => Path.GetRelativePath(_root, f)).OrderBy(f => f).ToList();

        files.Should().Equal(Path.Combine("a", "b", "two.jpg"), Path.Combine("a", "one.jpg"));
    }

    [Fact]
    public void GetFiles_Not_Recursive_Ignores_Subfolders()
    {
        File.WriteAllText(Path.Combine(_root, "top.jpg"), "x");

        var files = new FileService().GetFiles(_root, false, null).Select(Path.GetFileName).ToList();

        files.Should().Equal("top.jpg");
    }

    public void Dispose() => Directory.Delete(_root, true);
}
