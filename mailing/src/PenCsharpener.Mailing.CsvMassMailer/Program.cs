using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Models;
using PenCsharpener.Mailing.CsvMassMailer.Services;
using PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;
using Serilog;
using Serilog.Events;

namespace PenCsharpener.Mailing.CsvMassMailer;

public static class Program
{
    private static void Main(string[] args)
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

            var app = CoconaApp.CreateBuilder(args, ConfigureCocona)
                .AddServices()
                .Build();

            app.AddCommand(async (CoconaOptions options, ICsvMassMailerService service, CoconaAppContext context) =>
            {
                await service.ExecuteAsync(options);
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
        services.Services.RegisterApplicationServices();
        services.Services.RegisterInfrastructureServices();
        services.Services.AddTransient<ICsvMassMailerService, CsvMassMailerService>();

        return services;
    }

    public static void ConfigureCocona(CoconaAppOptions options)
    {
        options.EnableShellCompletionSupport = true;
    }
}
