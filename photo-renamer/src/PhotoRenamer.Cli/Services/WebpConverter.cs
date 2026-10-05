using PhotoRenamer.Cli.Services.Abstractions;
using SkiaSharp;

namespace PhotoRenamer.Cli.Services;

public class WebpConverter : IWebpConverter
{
    public string? Convert(string sourcePath, int quality)
    {
        var targetPath = Path.ChangeExtension(sourcePath, ".webp");

        if (File.Exists(targetPath))
        {
            return null;
        }

        using var codec = SKCodec.Create(sourcePath) ?? throw new InvalidDataException($"Cannot decode image '{sourcePath}'.");
        using var decoded = SKBitmap.Decode(codec);
        using var oriented = ApplyOrigin(decoded, codec.EncodedOrigin);
        using var image = SKImage.FromBitmap(oriented);
        using var data = image.Encode(SKEncodedImageFormat.Webp, quality);
        using var output = File.Create(targetPath);

        data.SaveTo(output);

        return targetPath;
    }

    // WebP written by SkiaSharp carries no EXIF, so the EXIF orientation is baked into the pixels.
    private static SKBitmap ApplyOrigin(SKBitmap source, SKEncodedOrigin origin)
    {
        float w = source.Width, h = source.Height;

        var (matrix, swap) = origin switch
        {
            SKEncodedOrigin.TopRight => (new SKMatrix(-1, 0, w, 0, 1, 0, 0, 0, 1), false),
            SKEncodedOrigin.BottomRight => (new SKMatrix(-1, 0, w, 0, -1, h, 0, 0, 1), false),
            SKEncodedOrigin.BottomLeft => (new SKMatrix(1, 0, 0, 0, -1, h, 0, 0, 1), false),
            SKEncodedOrigin.LeftTop => (new SKMatrix(0, 1, 0, 1, 0, 0, 0, 0, 1), true),
            SKEncodedOrigin.RightTop => (new SKMatrix(0, -1, h, 1, 0, 0, 0, 0, 1), true),
            SKEncodedOrigin.RightBottom => (new SKMatrix(0, -1, h, -1, 0, w, 0, 0, 1), true),
            SKEncodedOrigin.LeftBottom => (new SKMatrix(0, 1, 0, -1, 0, w, 0, 0, 1), true),
            _ => (SKMatrix.Identity, false),
        };

        var target = swap
            ? new SKBitmap(source.Height, source.Width, source.ColorType, source.AlphaType)
            : new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);

        using var canvas = new SKCanvas(target);
        canvas.SetMatrix(matrix);
        canvas.DrawBitmap(source, 0, 0);

        return target;
    }
}
