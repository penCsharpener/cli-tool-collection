using LineToBibleReference.Console.Abstractions;

namespace LineToBibleReference.Console;

public class Worker : BackgroundService
{
    private readonly IDataConverterFactory _converterFactory;
    private readonly IHostApplicationLifetime _host;
    private readonly ILogger<Worker> _logger;

    public Worker(IDataConverterFactory converterFactory, IHostApplicationLifetime host, ILogger<Worker> logger)
    {
        _converterFactory = converterFactory;
        _host = host;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var list = _converterFactory.GetDataConverter().ConvertToBibleReferences().ToBlockingEnumerable(stoppingToken).ToList();

        _logger.LogInformation("count: {count}", list.Count);
        list.GroupBy(r => new { r.BookAbbreviation, r.Chapter }).ToList().ForEach(g => _logger.LogInformation("{book} {chapter}", g.Key.BookAbbreviation, g.Key.Chapter));

        _host.StopApplication();

        await StopAsync(stoppingToken);
    }
}
