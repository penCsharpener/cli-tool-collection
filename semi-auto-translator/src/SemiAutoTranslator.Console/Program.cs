using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;
using SemiAutoTranslator.Console.Services;
using Serilog;

namespace SemiAutoTranslator.Console;

public class Program
{
    public static void Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder(args);
        hostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

        var host = hostBuilder.ConfigureServices((context, services) =>
         {
             var settings = context.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

             services.AddSingleton(settings);
             services.AddScoped<IFileService, TextFileService>();
             services.AddSingleton<IWebDriverFactory, FirefoxWebDriverFactory>();
             services.AddKeyedScoped<ITextDividerService, GutenbergTextDividerService>("Gutenberg");
             services.AddKeyedScoped<ITranslator, GutenbergTranslator>("Gutenberg");
             services.AddHostedService<Worker>();
         })
         .Build();

        host.Run();
    }
}