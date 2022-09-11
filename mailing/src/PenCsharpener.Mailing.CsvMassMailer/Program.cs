using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PenCsharpener.Mailing.Common.Services;
using PenCsharpener.Mailing.Common.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Extensions;
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
                .AddServices(args)
                .Build();

            app.AddCommand(async (CoconaOptions options, ICsvMassMailerService service, CoconaAppContext context) =>
            {
                await service.ExecuteAsync(options);
            });

            app.Services.GetRequiredService<ISmtpService>().SendEmailAsync("", "", "", "");

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

    public static CoconaAppBuilder AddServices(this CoconaAppBuilder services, string[]? args)
    {
        services.Host.UseSerilog();
        services.Services.RegisterApplicationServices();
        services.Services.RegisterInfrastructureServices();

        services.Services.AddTransient<ISmtpService>(sp =>
        {
            return args.ParseSmtpConfiguration() is { } smtpConfig
                ? new SmtpService(Options.Create(smtpConfig), sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SmtpService>>())
                : new NullSmtpService();
        });
        services.Services.AddTransient<ICsvMassMailerService, CsvMassMailerService>();

        return services;
    }

    public static void ConfigureCocona(CoconaAppOptions options)
    {
        options.EnableShellCompletionSupport = true;
    }
}
