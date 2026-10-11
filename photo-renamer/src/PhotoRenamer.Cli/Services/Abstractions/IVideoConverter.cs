namespace PhotoRenamer.Cli.Services.Abstractions;

public record VideoEncodeOptions(int Cq, string Preset, int AacBitrateKbps);

public enum VideoConversionStatus
{
    /// <summary>A new file was written to the target path; the source is untouched.</summary>
    Converted,

    /// <summary>The target already existed and was not overwritten.</summary>
    AlreadyExisted,

    /// <summary>The source carries the PhotoRenamer marker tag, i.e. it was produced by this tool, and is ignored.</summary>
    AlreadyConverted,

    /// <summary>The encoded file was not smaller than the source, so it was discarded and the source left untouched.</summary>
    NotSmaller,
}

public record VideoConversionResult(string OutputPath, VideoConversionStatus Status, bool IsValid, string? Detail = null);

public interface IVideoConverter
{
    /// <summary>Throws if ffmpeg or ffprobe cannot be started from PATH.</summary>
    Task EnsureToolsAvailableAsync(CancellationToken token);

    /// <summary>True if the file carries the marker tag written by a previous conversion.</summary>
    Task<bool> IsConvertedAsync(string path, CancellationToken token);

    /// <summary>
    /// Encodes the video to H.265 (NVENC) with AAC audio and writes it to <paramref name="targetPath"/>,
    /// leaving the source untouched. An existing target - also in another '*_converted_mp4' folder next to the
    /// source - is never overwritten, only checked. Already H.265 sources are encoded again to lower their bitrate;
    /// a result that is not smaller than the source is discarded.
    /// </summary>
    Task<VideoConversionResult> ConvertAsync(string sourcePath, string targetPath, VideoEncodeOptions options, CancellationToken token);
}
