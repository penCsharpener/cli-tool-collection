using PhotoRenamer.Cli.Services;
using SkiaSharp;

namespace PhotoRenamer.Cli.Tests;

public class WebpConverterTests : IDisposable
{
    private readonly string _directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;

    [Fact]
    public void Convert_Creates_Webp_Next_To_Jpg_And_Keeps_Original()
    {
        var jpg = Path.Combine(_directory, "test.jpg");
        CreateJpeg(jpg, 40, 20);

        var result = new WebpConverter().Convert(jpg, 70);

        result.WebpPath.Should().Be(Path.Combine(_directory, "test.webp"));
        result.AlreadyExisted.Should().BeFalse();
        result.IsValid.Should().BeTrue();
        File.Exists(jpg).Should().BeTrue();
        using var webp = SKBitmap.Decode(result.WebpPath);
        (webp.Width, webp.Height).Should().Be((40, 20));
    }

    [Fact]
    public void Convert_Does_Not_Overwrite_And_Flags_Invalid_Existing_Webp()
    {
        var jpg = Path.Combine(_directory, "test.jpg");
        CreateJpeg(jpg, 10, 10);
        File.WriteAllText(Path.Combine(_directory, "test.webp"), "existing");

        var result = new WebpConverter().Convert(jpg, 70);

        result.AlreadyExisted.Should().BeTrue();
        result.IsValid.Should().BeFalse();
        File.ReadAllText(result.WebpPath).Should().Be("existing");
    }

    [Fact]
    public void Convert_Accepts_Valid_Existing_Webp()
    {
        var jpg = Path.Combine(_directory, "test.jpg");
        CreateJpeg(jpg, 10, 10);
        var converter = new WebpConverter();
        converter.Convert(jpg, 70);

        var second = converter.Convert(jpg, 70);

        second.AlreadyExisted.Should().BeTrue();
        second.IsValid.Should().BeTrue();
    }

    private static void CreateJpeg(string path, int width, int height)
    {
        using var bitmap = new SKBitmap(width, height);
        bitmap.Erase(SKColors.CornflowerBlue);
        using var data = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Jpeg, 90);
        using var stream = File.Create(path);
        data.SaveTo(stream);
    }

    public void Dispose() => Directory.Delete(_directory, true);
}
