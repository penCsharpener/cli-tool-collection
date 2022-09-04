using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;
using PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Extensions;

namespace PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Tests;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Integration");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterInfrastructureServices();
        services.RegisterApplicationServices();
    }
}
