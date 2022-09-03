using System.Management.Automation;
using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using PhotoRename.Common.Services;
using PhotoRename.Common.Services.Abstractions;
using PhotoTimeStampShifter.Cli.Models;
using PhotoTimeStampShifter.Cli.Services;
using PhotoTimeStampShifter.Cli.Services.Abstractions;
using Serilog;
using Serilog.Events;

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

            app.AddCommand(async (RenameParameters options, ITimestampShifterService renameService, CoconaAppContext context) =>
            {
                if (string.IsNullOrWhiteSpace(options.TimeShiftValue))
                {
                    throw new ArgumentException($"{nameof(options.TimeShiftValue)} is required value", nameof(options.TimeShiftValue));
                }

                var cmdList = new List<string>();

                foreach (var line in renameService.RenameTimeStamp(options))
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
        services.Services.AddTransient<ITimestampShifterService, TimestampShifterService>();
        services.Services.AddTransient<IFileService, FileService>();

        return services;
    }
}

