using TextCompAs.Application.Abstractions;

namespace TextCompAs.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var textProvider = scope.ServiceProvider.GetRequiredService<ITextProvider>();

        var text = await textProvider.GetTextAsync(stoppingToken);

        Console.WriteLine(text);
    }
}
