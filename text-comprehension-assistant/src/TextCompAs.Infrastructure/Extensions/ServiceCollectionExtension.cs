using Microsoft.Extensions.DependencyInjection;
using TextCompAs.Application.Abstractions;
using TextCompAs.Application.Models;
using TextCompAs.Infrastructure.Services;

namespace TextCompAs.Infrastructure.Extensions;
public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient<GutenbergTextDownloader>();
        services.AddScoped<ITextProvider>(sp =>
        {
            var settings = sp.GetRequiredService<AppSettings>();

            if (settings.SourcePath.StartsWith("https://www.gutenberg.org"))
            {
                return sp.GetRequiredService<GutenbergTextDownloader>();
            }

            throw new NotSupportedException("This way of obtaining text is not supported.");
        });

        return services;
    }
}
