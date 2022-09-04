using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.Common.Services;
using PenCsharpener.Mailing.Common.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<ISmtpService, SmtpService>();
    }
}
