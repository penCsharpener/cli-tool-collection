using System.Text;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;
using LineToBibleReference.Console.Services;
using Serilog;

namespace LineToBibleReference.Console;

public class Program
{
    public static void Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.Unicode;

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(RegisterServices)
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
            .Build();

        host.Run();
    }

    private static void RegisterServices(HostBuilderContext context, IServiceCollection services)
    {
        var settings = context.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;
        services.AddSingleton(settings);
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IDataConverterFactory, DataConverterFactory>();
        services.AddSingleton<ICharacterStatistics, CharacterStatistics>();
        services.AddScoped<GermanTextToDataConverter>();
        services.AddScoped<HebrewTextToDataConverter>();
        services.AddScoped<GreekTextToDataConverter>();
        services.AddHostedService<Worker>();
    }
}