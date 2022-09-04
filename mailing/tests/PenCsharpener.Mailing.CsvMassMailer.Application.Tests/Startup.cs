using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Integration");
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterApplicationServices();
    }
}
