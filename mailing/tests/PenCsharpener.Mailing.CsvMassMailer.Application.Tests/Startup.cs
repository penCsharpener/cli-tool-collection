using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IFileSystem, FileSystem>();
        services.AddTransient<ICsvReader, CsvReader>();
    }
}
