namespace PhotoRenamer.Cli.Services.Abstractions;

public record VideoEncodeOptions(int Cq, string Preset, int AacBitrateKbps);

public enum VideoConversionStatus
{
    /// <summary>A new file was written to the target path; the source is untouched.</summary>
    Converted,

    /// <summary>The target already existed and was not overwritten.</summary>
    AlreadyExisted,

    /// <summary>Target and source are the same path: the source was replaced by the encoded file.</summary>
    ReplacedSource,

    /// <summary>The source carries the PhotoRenamer marker tag, i.e. it was produced by this tool, and is ignored.</summary>
    AlreadyConverted,

    /// <summary>The source is already H.265 and has the target name; nothing to do.</summary>
    AlreadyHevc,
}

public record VideoConversionResult(string OutputPath, VideoConversionStatus Status, bool IsValid);

public interface IVideoConverter
{
    /// <summary>Throws if ffmpeg or ffprobe cannot be started from PATH.</summary>
    Task EnsureToolsAvailableAsync(CancellationToken token);

    /// <summary>True if the file carries the marker tag written by a previous conversion.</summary>
    Task<bool> IsConvertedAsync(string path, CancellationToken token);

    /// <summary>
    /// Encodes the video to H.265 (NVENC) with AAC audio and writes it to <paramref name="targetPath"/>,
    /// leaving the source untouched. An existing target is never overwritten, only checked.
    /// If the target is the source itself, it is only replaced when <paramref name="allowReplaceSource"/> is set
    /// and the encoded file passed validation; otherwise an exception is thrown.
    /// </summary>
    Task<VideoConversionResult> ConvertAsync(string sourcePath, string targetPath, VideoEncodeOptions options, bool allowReplaceSource, CancellationToken token);
}
