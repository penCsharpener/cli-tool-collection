using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class DataConverterFactory : IDataConverterFactory
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AppSettings _settings;

    public DataConverterFactory(IServiceScopeFactory scopeFactory, AppSettings settings)
    {
        _scopeFactory = scopeFactory;
        _settings = settings;
    }

    public ITextToDataConverter GetDataConverter()
    {
        using var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        return _settings.SelectedRegexPattern switch
        {
            "de" => serviceProvider.GetRequiredService<GermanTextToDataConverter>(),
            "heb" => serviceProvider.GetRequiredService<HebrewTextToDataConverter>(),
            "gr" => serviceProvider.GetRequiredService<GreekTextToDataConverter>()
        };
    }
}
