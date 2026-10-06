using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class FfmpegVideoConverter : IVideoConverter
{
    public async Task EnsureToolsAvailableAsync(CancellationToken token)
    {
        foreach (var tool in new[] { "ffmpeg", "ffprobe" })
        {
            var (exitCode, _, _) = await RunAsync(tool, ["-version"], token);

            if (exitCode != 0)
            {
                throw new InvalidOperationException($"'{tool}' returned exit code {exitCode}.");
            }
        }
    }

    public async Task<bool> IsConvertedAsync(string path, CancellationToken token)
    {
        try
        {
            return (await ProbeAsync(path, token)).IsMarked;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return false; // unreadable file or no ffprobe: treat as not converted, the conversion reports the real problem
        }
    }

    public async Task<VideoConversionResult> ConvertAsync(string sourcePath, string targetPath, VideoEncodeOptions options, bool allowReplaceSource, CancellationToken token)
    {
        var sameFile = string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(targetPath), OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        var probe = await ProbeAsync(sourcePath, token);

        if (probe.IsMarked)
        {
            return new VideoConversionResult(sourcePath, VideoConversionStatus.AlreadyConverted, false);
        }

        if (sameFile)
        {
            if (string.Equals(probe.CodecName, "hevc", StringComparison.OrdinalIgnoreCase))
            {
                return new VideoConversionResult(targetPath, VideoConversionStatus.AlreadyHevc, false);
            }

            if (!allowReplaceSource)
            {
                throw new InvalidOperationException("the encoded file would get the same name as the original. Use --delete-original to replace the original.");
            }
        }
        else if (File.Exists(targetPath))
        {
            return new VideoConversionResult(targetPath, VideoConversionStatus.AlreadyExisted, await IsValidCopyAsync(sourcePath, targetPath, token));
        }

        var times = FileTimes.Read(sourcePath);
        var partPath = Path.ChangeExtension(targetPath, ".part.mp4");

        try
        {
            var (exitCode, _, error) = await RunAsync("ffmpeg", FfmpegArguments.BuildEncode(sourcePath, partPath, probe, options), token);

            if (exitCode != 0)
            {
                throw new InvalidOperationException($"ffmpeg failed with exit code {exitCode}: {error.Trim()}");
            }

            var valid = await IsValidCopyAsync(sourcePath, partPath, token);

            if (sameFile && !valid)
            {
                throw new InvalidOperationException("the encoded file failed validation, the original was kept.");
            }

            File.Move(partPath, targetPath, overwrite: sameFile);
            FileTimes.Apply(targetPath, times);

            return new VideoConversionResult(targetPath, sameFile ? VideoConversionStatus.ReplacedSource : VideoConversionStatus.Converted, valid);
        }
        finally
        {
            if (File.Exists(partPath))
            {
                File.Delete(partPath);
            }
        }
    }

    private static async Task<bool> IsValidCopyAsync(string sourcePath, string targetPath, CancellationToken token)
    {
        try
        {
            var source = await ProbeAsync(sourcePath, token);
            var target = await ProbeAsync(targetPath, token);

            if (!string.Equals(target.CodecName, "hevc", StringComparison.OrdinalIgnoreCase) || source.DurationSeconds is null || target.DurationSeconds is null)
            {
                return false;
            }

            var tolerance = Math.Max(1.0, source.DurationSeconds.Value * 0.01);

            return Math.Abs(source.DurationSeconds.Value - target.DurationSeconds.Value) <= tolerance;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return false;
        }
    }

    private static async Task<VideoProbe> ProbeAsync(string path, CancellationToken token)
    {
        var (exitCode, output, error) = await RunAsync("ffprobe",
        [
            "-v", "error", "-select_streams", "v:0",
            "-show_entries", "stream=codec_name,pix_fmt,color_range,color_space,color_transfer,color_primaries:format=duration:format_tags",
            "-of", "json", path,
        ], token);

        if (exitCode != 0)
        {
            throw new InvalidOperationException($"ffprobe failed for '{path}': {error.Trim()}");
        }

        using var document = JsonDocument.Parse(output);
        var root = document.RootElement;
        var stream = root.TryGetProperty("streams", out var streams) && streams.GetArrayLength() > 0 ? streams[0] : default;

        string? Get(JsonElement element, string name) =>
            element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var value) ? value.GetString() : null;

        double? duration = null;

        if (root.TryGetProperty("format", out var format) && double.TryParse(Get(format, "duration"), NumberStyles.Float, CultureInfo.InvariantCulture, out var seconds))
        {
            duration = seconds;
        }

        var marked = false;

        if (root.TryGetProperty("format", out var formatElement) && formatElement.TryGetProperty("tags", out var tags) && tags.ValueKind == JsonValueKind.Object)
        {
            marked = tags.EnumerateObject().Any(tag => tag.Name.Equals(FfmpegArguments.MarkerTag, StringComparison.OrdinalIgnoreCase));
        }

        return new VideoProbe(Get(stream, "pix_fmt"), Get(stream, "color_range"), Get(stream, "color_space"), Get(stream, "color_transfer"), Get(stream, "color_primaries"), duration, Get(stream, "codec_name"), marked);
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunAsync(string fileName, IEnumerable<string> arguments, CancellationToken token)
    {
        var startInfo = new ProcessStartInfo(fileName)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException($"Could not start '{fileName}'. Make sure it is installed and available in PATH.", ex);
        }

        // read both streams so the process cannot block on a full pipe
        var standardOutput = process.StandardOutput.ReadToEndAsync(token);
        var standardError = process.StandardError.ReadToEndAsync(token);

        try
        {
            await process.WaitForExitAsync(token);
        }
        catch (OperationCanceledException)
        {
            process.Kill(entireProcessTree: true);
            throw;
        }

        return (process.ExitCode, await standardOutput, await standardError);
    }
}
