using Microsoft.Extensions.DependencyInjection;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Services;
using PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterApplicationServices();
        services.AddTransient<ICsvMassMailerService, CsvMassMailerService>();
    }
}
