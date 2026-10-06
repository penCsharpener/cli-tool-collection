using Directory = System.IO.Directory;
using MetadataExtractor;
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

    [Fact]
    public void Convert_Keeps_Exif_Resets_Orientation_And_Copies_File_Times()
    {
        var jpg = Path.Combine(_directory, "exif.jpg");
        CreateJpeg(jpg, 40, 20);
        InjectExif(jpg);
        var modified = new DateTime(2020, 5, 6, 7, 8, 9, DateTimeKind.Utc);
        File.SetCreationTimeUtc(jpg, modified.AddDays(-1)); // first: on Unix this also touches the write time
        File.SetLastWriteTimeUtc(jpg, modified);

        var result = new WebpConverter().Convert(jpg, 70);

        result.IsValid.Should().BeTrue();
        var directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(result.WebpPath);
        var ifd0 = directories.OfType<MetadataExtractor.Formats.Exif.ExifIfd0Directory>().Single();
        ifd0.GetString(MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagDateTime).Should().Be("2024:01:02 03:04:05");
        ifd0.GetInt32(MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagOrientation).Should().Be(1);
        File.GetLastWriteTimeUtc(result.WebpPath).Should().Be(modified);

        if (OperatingSystem.IsWindows())
        {
            File.GetCreationTimeUtc(result.WebpPath).Should().Be(modified.AddDays(-1));
        }
    }

    // inserts an APP1/EXIF segment (orientation 6, DateTime) right after the JPEG SOI marker
    private static void InjectExif(string path)
    {
        var tiff = new List<byte> { 0x49, 0x49, 0x2A, 0x00, 0x08, 0x00, 0x00, 0x00, 0x02, 0x00 };
        tiff.AddRange(new byte[] { 0x12, 0x01, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00 });
        tiff.AddRange(new byte[] { 0x32, 0x01, 0x02, 0x00, 0x14, 0x00, 0x00, 0x00, 0x26, 0x00, 0x00, 0x00 });
        tiff.AddRange(new byte[] { 0, 0, 0, 0 });
        tiff.AddRange(System.Text.Encoding.ASCII.GetBytes("2024:01:02 03:04:05\0"));

        var payload = System.Text.Encoding.ASCII.GetBytes("Exif\0\0").Concat(tiff).ToArray();
        var length = payload.Length + 2;
        var segment = new byte[] { 0xFF, 0xE1, (byte)(length >> 8), (byte)length }.Concat(payload).ToArray();

        var original = File.ReadAllBytes(path);
        File.WriteAllBytes(path, original.Take(2).Concat(segment).Concat(original.Skip(2)).ToArray());
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
