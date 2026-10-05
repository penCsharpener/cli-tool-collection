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
                    Console.WriteLine("1.0.5");
                }

                var cmdList = new List<string>();
                var taskList = new List<Task>();

                var shell = ShellDetector.Resolve(options);

                using var ps = shell == ShellType.PowerShell ? System.Management.Automation.PowerShell.Create() : null;

                await foreach (var line in renameService.GetNameCommandsAsync(options, context.CancellationToken))
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

                        if (options.ConvertToWebp && IsJpeg(line.NewFileInfo.Name))
                        {
                            var webpPath = webpConverter.Convert(line.NewFileInfo.FullName, options.WebpQuality);
                            Console.WriteLine(webpPath is null
                                ? $"\t\t\t\t\twebp already exists, skipped: {line.NewFileInfo.Name}"
                                : $"\t\t\t\t\twebp: {Path.GetFileName(webpPath)}");
                        }
                    }
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