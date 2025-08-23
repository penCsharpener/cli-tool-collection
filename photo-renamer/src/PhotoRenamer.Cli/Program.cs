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

            app.AddCommand(async (RenameParameters options, IRenameService renameService, CoconaAppContext context) =>
            {
                if (options.PrintVersion)
                {
                    Console.WriteLine("1.0.5");
                }

                var cmdList = new List<string>();
                var taskList = new List<Task>();

                using var ps = System.Management.Automation.PowerShell.Create();

                await foreach (var line in renameService.GetNameCommandsAsync(options, context.CancellationToken))
                {
                    if (options.ExecuteRename)
                    {
                        Console.WriteLine($"\"{line.FileInfo.DirectoryName}\":   {line.FileInfo.Name} ==> {line.NewFileInfo.Name}");
                    }
                    else
                    {
                        Console.WriteLine(options.PreferCmd ? line.CmdRenameCommand : line.PowershellRenameCommand);
                    }

                    if (options.ExecuteRename && !options.PreferCmd)
                    {
                        if (context.CancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        ps.AddScript(line.PowershellRenameCommand);

                        var pipelineObjects = await ps.InvokeAsync();

                        ps.Commands.Clear();
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

    public static CoconaAppBuilder AddServices(this CoconaAppBuilder services)
    {
        services.Host.UseSerilog();
        services.Services.AddSingleton<IRenameService, RenameService>();
        services.Services.AddSingleton<IFileService, FileService>();
        services.Services.AddSingleton<IImageSharpWrapper, ImageSharpWrapper>();
        services.Services.AddSingleton<IFileNameStrategyFactory, FileNameStrategyFactory>();

        return services;
    }
}