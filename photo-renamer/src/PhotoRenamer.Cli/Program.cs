using System.Reflection;
using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using PhotoRename.Common.Services;
using PhotoRename.Common.Services.Abstractions;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services;
using PhotoRenamer.Cli.Services.Abstractions;
using Serilog;
using Serilog.Events;

namespace PhotoRenamer.Cli;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(string.Join(",", args));

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting console app");

            var app = CoconaApp.CreateBuilder()
                .AddServices()
                .Build();

            app.AddCommand(async (RenameParameters options, IRenameService renameService, IWebpConverter webpConverter, IVideoConverter videoConverter, CoconaAppContext context) =>
            {
                if (options.PrintVersion)
                {
                    Console.WriteLine("1.3.0");
                }

                if (options.DeleteOriginal && !options.ConvertToWebp && !options.ConvertToH265)
                {
                    Console.WriteLine("--delete-original requires --webp and/or --h265.");
                    return;
                }

                if (options.ConvertToH265 && options.ExecuteRename)
                {
                    try
                    {
                        await videoConverter.EnsureToolsAvailableAsync(context.CancellationToken);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        Console.WriteLine($"--h265 needs ffmpeg and ffprobe: {ex.Message}");
                        return;
                    }
                }

                var cmdList = new List<string>();
                var taskList = new List<Task>();

                var shell = ShellDetector.Resolve(options);
                var webpSources = new List<string>();
                var videoSources = new List<(string Source, string Target)>();

                using var ps = shell == ShellType.PowerShell ? System.Management.Automation.PowerShell.Create() : null;

                await foreach (var line in renameService.GetNameCommandsAsync(options, context.CancellationToken))
                {
                    // with --h265 the original video is never renamed: the encoded file gets the new name instead
                    var encodeOnly = options.ConvertToH265 && IsVideo(line.FileInfo.Name);

                    // files written by this tool carry a marker tag and are ignored
                    if (encodeOnly && await videoConverter.IsConvertedAsync(line.FileInfo.FullName, context.CancellationToken))
                    {
                        if (options.VerboseLogging)
                        {
                            Console.WriteLine($"\t\t\t\t\tignored, already converted by PhotoRenamer: {line.FileInfo.Name}");
                        }

                        continue;
                    }

                    if (encodeOnly && !line.IsAlreadyNamed)
                    {
                        Console.WriteLine($"\"{line.FileInfo.DirectoryName}\":   {line.FileInfo.Name} ==> {Path.ChangeExtension(line.NewFileInfo.Name, ".mp4")} (encoded to H.265, original is kept)");
                    }

                    if (!line.IsAlreadyNamed && !encodeOnly)
                    {
                        if (options.ExecuteRename)
                        {
                            Console.WriteLine($"\"{line.FileInfo.DirectoryName}\":   {line.FileInfo.Name} ==> {line.NewFileInfo.Name}");
                        }
                        else
                        {
                            Console.WriteLine(shell switch
                            {
                                ShellType.Bash => line.BashRenameCommand,
                                ShellType.Cmd => line.CmdRenameCommand,
                                _ => line.PowershellRenameCommand,
                            });
                        }

                        if (options.ExecuteRename && shell != ShellType.Cmd)
                        {
                            if (context.CancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            if (ps is null)
                            {
                                // equivalent of 'mv' without spawning a shell; does not overwrite existing files
                                File.Move(line.FileInfo.FullName, line.NewFileInfo.FullName);
                            }
                            else
                            {
                                ps.AddScript(line.PowershellRenameCommand);

                                var pipelineObjects = await ps.InvokeAsync();

                                ps.Commands.Clear();
                            }
                        }
                    }

                    var renamed = line.IsAlreadyNamed || shell != ShellType.Cmd;

                    if (options.ExecuteRename && renamed && options.ConvertToWebp && IsJpeg(line.NewFileInfo.Name))
                    {
                        webpSources.Add(line.NewFileInfo.FullName);
                    }

                    if (options.ExecuteRename && encodeOnly)
                    {
                        videoSources.Add((line.FileInfo.FullName, Path.ChangeExtension(line.NewFileInfo.FullName, ".mp4")));
                    }
                }

                var deleteOriginals = (webpSources.Count + videoSources.Count) > 0 && ConfirmDeletion(options, webpSources.Count + videoSources.Count);

                if (webpSources.Count > 0)
                {
                    var threads = options.WebpThreads > 0 ? options.WebpThreads : Math.Max(1, Environment.ProcessorCount * 3 / 4);
                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = threads, CancellationToken = context.CancellationToken };

                    Console.WriteLine($"Converting {webpSources.Count} file(s) to webp using {threads} thread(s)");

                    await Parallel.ForEachAsync(webpSources, parallelOptions, (source, _) =>
                    {
                        try
                        {
                            var result = webpConverter.Convert(source, options.WebpQuality);
                            var name = Path.GetFileName(source);

                            Console.WriteLine(result.AlreadyExisted
                                ? $"\t\t\t\t\twebp already exists, not overwritten: {Path.GetFileName(result.WebpPath)}"
                                : $"\t\t\t\t\twebp: {Path.GetFileName(result.WebpPath)}");

                            // the original is only deleted if a valid webp (new or already existing) is verifiably there
                            if (deleteOriginals)
                            {
                                if (result.IsValid)
                                {
                                    File.Delete(source);
                                    Console.WriteLine($"\t\t\t\t\tdeleted original: {name}");
                                }
                                else
                                {
                                    Console.WriteLine($"\t\t\t\t\toriginal kept, webp is not a valid copy: {name}");
                                }
                            }
                        }
                        catch (Exception ex) when (!options.NoErrorLogging)
                        {
                            Console.WriteLine($"\t\t\t\t\twebp conversion failed for {Path.GetFileName(source)}: {ex.Message}");
                        }
                        catch (Exception)
                        {
                            // errors are suppressed by --no-err
                        }

                        return ValueTask.CompletedTask;
                    });
                }

                if (videoSources.Count > 0)
                {
                    var encodeOptions = new VideoEncodeOptions(options.H265Cq, options.H265Preset, options.AacBitrate);
                    var videoThreads = Math.Max(1, options.H265Threads);
                    var videoParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = videoThreads, CancellationToken = context.CancellationToken };

                    Console.WriteLine($"Encoding {videoSources.Count} video(s) to H.265 using {videoThreads} parallel encode(s)");

                    await Parallel.ForEachAsync(videoSources, videoParallelOptions, async (item, token) =>
                    {
                        var (source, target) = item;
                        var name = Path.GetFileName(source);

                        try
                        {
                            var result = await videoConverter.ConvertAsync(source, target, encodeOptions, deleteOriginals, token);
                            var outputName = Path.GetFileName(result.OutputPath);

                            Console.WriteLine(result.Status switch
                            {
                                VideoConversionStatus.AlreadyExisted => $"\t\t\t\t\th265 already exists, not overwritten: {outputName}",
                                VideoConversionStatus.AlreadyConverted => $"\t\t\t\t\tignored, already converted by PhotoRenamer: {name}",
                                VideoConversionStatus.AlreadyHevc => $"\t\t\t\t\talready H.265, skipped: {name}",
                                VideoConversionStatus.ReplacedSource => $"\t\t\t\t\th265 replaced original: {outputName}",
                                _ => $"\t\t\t\t\th265: {name} ==> {outputName}",
                            });

                            // the original is only deleted if a valid H.265 file (new or already existing) is verifiably there
                            if (deleteOriginals && result.Status is VideoConversionStatus.Converted or VideoConversionStatus.AlreadyExisted)
                            {
                                if (result.IsValid)
                                {
                                    File.Delete(source);
                                    Console.WriteLine($"\t\t\t\t\tdeleted original: {name}");
                                }
                                else
                                {
                                    Console.WriteLine($"\t\t\t\t\toriginal kept, h265 file is not a valid copy: {name}");
                                }
                            }
                        }
                        catch (Exception ex) when (ex is not OperationCanceledException && !options.NoErrorLogging)
                        {
                            Console.WriteLine($"\t\t\t\t\th265 encoding failed for {name}: {ex.Message}");
                        }
                        catch (Exception ex) when (ex is not OperationCanceledException)
                        {
                            // errors are suppressed by --no-err
                        }
                    });
                }
            });

            app.Run();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Operation cancelled.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Console app terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static bool ConfirmDeletion(RenameParameters options, int count)
    {
        if (!options.DeleteOriginal)
        {
            return false;
        }

        if (Console.IsInputRedirected)
        {
            Console.WriteLine("Cannot ask for confirmation because input is redirected. Original files will NOT be deleted.");
            return false;
        }

        Console.Write($"Delete the original file(s) ({count}) after successful conversion? This cannot be undone. Type 'yes' to confirm: ");

        if (string.Equals(Console.ReadLine()?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        Console.WriteLine("Not confirmed. Original files will be kept.");

        return false;
    }

    private static bool IsVideo(string fileName)
    {
        return fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith(".mov", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith(".avi", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsJpeg(string fileName)
    {
        return fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase);
    }

    public static CoconaAppBuilder AddServices(this CoconaAppBuilder services)
    {
        services.Host.UseSerilog();
        services.Services.AddSingleton<IRenameService, RenameService>();
        services.Services.AddSingleton<IFileService, FileService>();
        services.Services.AddSingleton<IImageMetadataWrapper, ImageMetadataWrapper>();
        services.Services.AddSingleton<IWebpConverter, WebpConverter>();
        services.Services.AddSingleton<IVideoConverter, FfmpegVideoConverter>();
        services.Services.AddSingleton<IFileNameStrategyFactory, FileNameStrategyFactory>();

        return services;
    }
}