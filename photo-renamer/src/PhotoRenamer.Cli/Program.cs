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

            app.AddCommand(async (RenameParameters options, IRenameService renameService, IWebpConverter webpConverter, CoconaAppContext context) =>
            {
                if (options.PrintVersion)
                {
                    Console.WriteLine("1.1.1");
                }

                if (options.DeleteOriginal && !options.ConvertToWebp)
                {
                    Console.WriteLine("--delete-original requires --webp.");
                    return;
                }

                var cmdList = new List<string>();
                var taskList = new List<Task>();

                var shell = ShellDetector.Resolve(options);
                var webpSources = new List<string>();

                using var ps = shell == ShellType.PowerShell ? System.Management.Automation.PowerShell.Create() : null;

                await foreach (var line in renameService.GetNameCommandsAsync(options, context.CancellationToken))
                {
                    if (!line.IsAlreadyNamed)
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
                }

                if (webpSources.Count > 0)
                {
                    var threads = options.WebpThreads > 0 ? options.WebpThreads : Math.Max(1, Environment.ProcessorCount * 3 / 4);
                    var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = threads, CancellationToken = context.CancellationToken };

                    var deleteOriginals = ConfirmDeletion(options, webpSources.Count);

                    Console.WriteLine($"Converting {webpSources.Count} file(s) to webp using {threads} thread(s)");

                    await Parallel.ForEachAsync(webpSources, parallelOptions, (source, _) =>
                    {
                        try
                        {
                            var webpPath = webpConverter.Convert(source, options.WebpQuality);
                            Console.WriteLine(webpPath is null
                                ? $"\t\t\t\t\twebp already exists, skipped: {Path.GetFileName(source)}"
                                : $"\t\t\t\t\twebp: {Path.GetFileName(webpPath)}");

                            // only delete once the new file is verifiably there; an already existing webp is never trusted
                            if (deleteOriginals && webpPath is not null && new FileInfo(webpPath).Length > 0)
                            {
                                File.Delete(source);
                                Console.WriteLine($"\t\t\t\t\tdeleted original: {Path.GetFileName(source)}");
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

        Console.Write($"Delete the original jpg file(s) ({count}) after successful webp conversion? This cannot be undone. Type 'yes' to confirm: ");

        if (string.Equals(Console.ReadLine()?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        Console.WriteLine("Not confirmed. Original files will be kept.");

        return false;
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
        services.Services.AddSingleton<IFileNameStrategyFactory, FileNameStrategyFactory>();

        return services;
    }
}