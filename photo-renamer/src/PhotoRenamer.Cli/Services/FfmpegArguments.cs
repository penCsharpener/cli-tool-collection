namespace PhotoRenamer.Cli.Services;

public record VideoProbe(string? PixFmt, string? ColorRange, string? ColorSpace, string? ColorTransfer, string? ColorPrimaries, double? DurationSeconds, string? CodecName = null, bool IsMarked = false);

public static class FfmpegArguments
{
    /// <summary>Container tag written into every file this tool encodes, so later runs can recognise and ignore it.</summary>
    public const string MarkerTag = "photorenamer";
    public const string MarkerValue = "h265";

    /// <summary>
    /// HEVC via NVENC in constant quality mode. Frame rate stays untouched (no -r, timestamps passed through),
    /// colour range/matrix/primaries/transfer are pinned to what the source declares.
    /// </summary>
    public static List<string> BuildEncode(string input, string output, VideoProbe probe, Abstractions.VideoEncodeOptions options)
    {
        var args = new List<string>
        {
            "-hide_banner", "-nostdin", "-loglevel", "error", "-nostats",
            "-i", input,
            "-map", "0:v:0", "-map", "0:a?",
            "-map_metadata", "0",
            "-c:v", "hevc_nvenc",
            "-rc", "vbr", "-cq", options.Cq.ToString(), "-b:v", "0",
            "-preset", options.Preset,
            "-tag:v", "hvc1",
            "-fps_mode", "passthrough",
        };

        if (probe.PixFmt is not null && probe.PixFmt.Contains("10"))
        {
            args.AddRange(["-pix_fmt", "p010le", "-profile:v", "main10"]);
        }

        AddIfKnown(args, "-color_range", probe.ColorRange);
        AddIfKnown(args, "-colorspace", probe.ColorSpace);
        AddIfKnown(args, "-color_primaries", probe.ColorPrimaries);
        AddIfKnown(args, "-color_trc", probe.ColorTransfer);

        args.AddRange(["-metadata", $"{MarkerTag}={MarkerValue}"]);
        args.AddRange(["-c:a", "aac", "-b:a", $"{options.AacBitrateKbps}k"]);
        args.AddRange(["-movflags", "+faststart+use_metadata_tags", "-f", "mp4", output]);

        return args;
    }

    private static void AddIfKnown(List<string> args, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value) && !value.Equals("unknown", StringComparison.OrdinalIgnoreCase) && !value.Equals("unspecified", StringComparison.OrdinalIgnoreCase))
        {
            args.AddRange([name, value]);
        }
    }
}
