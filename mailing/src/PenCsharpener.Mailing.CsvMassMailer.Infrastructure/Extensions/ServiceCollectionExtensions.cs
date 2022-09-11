using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Services;

namespace PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IFileService, FileService>();
    }
}
