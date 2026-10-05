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

        result.Should().Be(Path.Combine(_directory, "test.webp"));
        File.Exists(jpg).Should().BeTrue();
        using var webp = SKBitmap.Decode(result!);
        (webp.Width, webp.Height).Should().Be((40, 20));
    }

    [Fact]
    public void Convert_Returns_Null_If_Webp_Exists()
    {
        var jpg = Path.Combine(_directory, "test.jpg");
        CreateJpeg(jpg, 10, 10);
        File.WriteAllText(Path.Combine(_directory, "test.webp"), "existing");

        new WebpConverter().Convert(jpg, 70).Should().BeNull();
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
