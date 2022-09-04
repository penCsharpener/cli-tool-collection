using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ICsvReader, CsvReader>();
        services.AddTransient<IEmailReplacementService, EmailReplacementService>();
        services.AddTransient<IFileSystem, FileSystem>();
    }
}
