using System.Buffers.Binary;
using System.Text;

namespace PhotoRenamer.Cli.Services;

/// <summary>Copies EXIF from a JPEG into a WebP (SkiaSharp cannot write metadata).</summary>
public static class WebpMetadata
{
    private const int ExifFlag = 0x08;
    private const ushort OrientationTag = 0x0112;

    /// <summary>Returns the raw TIFF-structured EXIF block of a JPEG (without the 'Exif\0\0' prefix), or null.</summary>
    public static byte[]? ReadJpegExif(string path)
    {
        var bytes = File.ReadAllBytes(path);

        if (bytes.Length < 4 || bytes[0] != 0xFF || bytes[1] != 0xD8)
        {
            return null;
        }

        var pos = 2;

        while (pos + 4 <= bytes.Length && bytes[pos] == 0xFF)
        {
            var marker = bytes[pos + 1];

            if (marker == 0xFF)
            {
                pos++; // fill byte
                continue;
            }

            if (marker == 0xDA || marker == 0xD9)
            {
                break; // start of scan / end of image: no more metadata segments
            }

            if (marker == 0x01 || (marker >= 0xD0 && marker <= 0xD7))
            {
                pos += 2; // standalone marker without length
                continue;
            }

            var length = BinaryPrimitives.ReadUInt16BigEndian(bytes.AsSpan(pos + 2));

            if (length < 2 || pos + 2 + length > bytes.Length)
            {
                break;
            }

            var payload = bytes.AsSpan(pos + 4, length - 2);

            if (marker == 0xE1 && payload.Length > 6 && payload[..6].SequenceEqual("Exif\0\0"u8))
            {
                return payload[6..].ToArray();
            }

            pos += 2 + length;
        }

        return null;
    }

    /// <summary>
    /// Sets the orientation tag to 1 (top-left). The converter already rotated the pixels,
    /// so the original value would make viewers rotate the image a second time.
    /// </summary>
    public static void ResetOrientation(byte[] tiff)
    {
        if (tiff.Length < 8)
        {
            return;
        }

        var littleEndian = tiff[0] == (byte)'I' && tiff[1] == (byte)'I';

        if (!littleEndian && !(tiff[0] == (byte)'M' && tiff[1] == (byte)'M'))
        {
            return;
        }

        ushort ReadU16(int offset) => littleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(tiff.AsSpan(offset)) : BinaryPrimitives.ReadUInt16BigEndian(tiff.AsSpan(offset));
        uint ReadU32(int offset) => littleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(tiff.AsSpan(offset)) : BinaryPrimitives.ReadUInt32BigEndian(tiff.AsSpan(offset));

        var ifd = (long)ReadU32(4);

        if (ifd + 2 > tiff.Length)
        {
            return;
        }

        var count = ReadU16((int)ifd);

        for (var i = 0; i < count; i++)
        {
            var entry = ifd + 2 + (i * 12);

            if (entry + 12 > tiff.Length)
            {
                return;
            }

            if (ReadU16((int)entry) == OrientationTag)
            {
                var value = (int)entry + 8;

                if (littleEndian)
                {
                    BinaryPrimitives.WriteUInt16LittleEndian(tiff.AsSpan(value), 1);
                }
                else
                {
                    BinaryPrimitives.WriteUInt16BigEndian(tiff.AsSpan(value), 1);
                }

                return;
            }
        }
    }

    /// <summary>Returns the WebP file bytes with an EXIF chunk added (upgrading a simple WebP to the extended VP8X layout).</summary>
    public static byte[] AddExif(byte[] webp, byte[] exif, int width, int height)
    {
        if (webp.Length < 20 || !webp.AsSpan(0, 4).SequenceEqual("RIFF"u8) || !webp.AsSpan(8, 4).SequenceEqual("WEBP"u8))
        {
            throw new InvalidDataException("Not a WebP file.");
        }

        var chunks = new List<(string Id, byte[] Payload)>();
        var pos = 12;

        while (pos + 8 <= webp.Length)
        {
            var id = Encoding.ASCII.GetString(webp, pos, 4);
            var size = (int)BinaryPrimitives.ReadUInt32LittleEndian(webp.AsSpan(pos + 4));

            if (pos + 8 + size > webp.Length)
            {
                throw new InvalidDataException("Truncated WebP chunk.");
            }

            chunks.Add((id, webp.AsSpan(pos + 8, size).ToArray()));
            pos += 8 + size + (size & 1);
        }

        var vp8x = chunks.FirstOrDefault(c => c.Id == "VP8X").Payload;

        if (vp8x is null)
        {
            vp8x = new byte[10];
            WriteU24(vp8x, 4, width - 1);
            WriteU24(vp8x, 7, height - 1);
        }

        vp8x[0] |= ExifFlag;

        chunks.RemoveAll(c => c.Id is "VP8X" or "EXIF");
        chunks.Insert(0, ("VP8X", vp8x));
        chunks.Add(("EXIF", exif));

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        writer.Write("RIFF"u8);
        writer.Write(0); // size, patched below
        writer.Write("WEBP"u8);

        foreach (var (id, payload) in chunks)
        {
            writer.Write(Encoding.ASCII.GetBytes(id));
            writer.Write((uint)payload.Length);
            writer.Write(payload);

            if ((payload.Length & 1) == 1)
            {
                writer.Write((byte)0);
            }
        }

        writer.Flush();

        var result = stream.ToArray();
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(4), (uint)(result.Length - 8));

        return result;
    }

    private static void WriteU24(byte[] buffer, int offset, int value)
    {
        buffer[offset] = (byte)value;
        buffer[offset + 1] = (byte)(value >> 8);
        buffer[offset + 2] = (byte)(value >> 16);
    }
}
