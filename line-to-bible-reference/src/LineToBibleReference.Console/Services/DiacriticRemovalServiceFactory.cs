using LineToBibleReference.Console.Abstractions;

namespace LineToBibleReference.Console.Services;

public sealed class DiacriticRemovalServiceFactory : IDiacriticRemovalServiceFactory
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DiacriticRemovalServiceFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IDiacriticRemovalService GetDiacriticRemovalService(string converterType)
    {
        using var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        return converterType switch
        {
            "elb" => serviceProvider.GetRequiredService<GermanDiacriticRemovalService>(),
            "heb" => serviceProvider.GetRequiredService<HebrewDiacriticRemovalService>(),
            "sbl" => serviceProvider.GetRequiredService<GreekDiacriticRemovalService>(),
            "esv" => serviceProvider.GetRequiredService<GermanDiacriticRemovalService>(),
            "kjv" => serviceProvider.GetRequiredService<GermanDiacriticRemovalService>()
        };
    }
}
