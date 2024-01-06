using TextCompAs.Application.Extensions;
using TextCompAs.Application.Models;
using TextCompAs.Infrastructure.Extensions;

namespace TextCompAs.Worker.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>() ?? throw new ArgumentNullException("AppSettings");

        services.AddSingleton(settings!)
            .RegisterApplicationServices()
            .RegisterInfrastructureServices()
            .RegisterWorkerServices();
        services.AddHostedService<Worker>();

        return services;
    }

    public static IServiceCollection RegisterWorkerServices(this IServiceCollection services)
    {

        return services;
    }
}
