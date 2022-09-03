using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IFileSystem, FileSystem>();
    }
}
