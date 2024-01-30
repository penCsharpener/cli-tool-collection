using SemiAutoTranslator.Console.Abstractions;

namespace SemiAutoTranslator.Console;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(IHostApplicationLifetime lifetime, IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
    {
        _lifetime = lifetime;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredKeyedService<ITranslator>("Gutenberg");

        await service.TranslateAsync(stoppingToken).ConfigureAwait(false);
    }
}
