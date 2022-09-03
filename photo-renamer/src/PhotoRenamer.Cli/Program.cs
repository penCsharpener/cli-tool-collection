using System.Management.Automation;
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
                var cmdList = new List<string>();

                await foreach (var line in renameService.GetNameCommandsAsync(options, context.CancellationToken))
                {
                    Console.WriteLine(line);

                    cmdList.Add(line);
                }

                if (options.ExecuteRename && !options.PreferCmd)
                {
                    using var ps = PowerShell.Create();

                    foreach (var line in cmdList)
                    {
                        if (context.CancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        ps.AddScript(line);

                        var pipelineObjects = await ps.InvokeAsync();

                        ps.Commands.Clear();
                    }
                }
            });

            app.Run();
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
        services.Services.AddTransient<IRenameService, RenameService>();
        services.Services.AddTransient<IFileService, FileService>();

        return services;
    }
}