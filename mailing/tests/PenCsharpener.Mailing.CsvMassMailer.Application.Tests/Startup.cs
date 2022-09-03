using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PenCsharpener.Mailing.CsvMassMailer.Application.Extensions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterApplicationServices();
        services.AddTransient(_ =>
        {
            var proxy = Substitute.For<IHostEnvironment>();

            proxy.ContentRootPath.Returns(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            return proxy;
        });
    }
}
