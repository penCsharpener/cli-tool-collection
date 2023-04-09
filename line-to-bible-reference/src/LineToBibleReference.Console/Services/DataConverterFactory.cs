using LineToBibleReference.Console.Abstractions;

namespace LineToBibleReference.Console.Services;

public sealed class DataConverterFactory : IDataConverterFactory
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DataConverterFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public ITextToDataConverter GetDataConverter(string converterType)
    {
        using var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        return converterType switch
        {
            "elb" => serviceProvider.GetRequiredService<GermanTextToDataConverter>(),
            "heb" => serviceProvider.GetRequiredService<HebrewTextToDataConverter>(),
            "sbl" => serviceProvider.GetRequiredService<GreekTextToDataConverter>(),
            "esv" => serviceProvider.GetRequiredService<EsvTextToDataConverter>(),
            "kjv" => serviceProvider.GetRequiredService<KjvTextToDataConverter>()
        };
    }
}
