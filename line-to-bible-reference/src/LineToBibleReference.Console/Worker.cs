using LineToBibleReference.Console.Abstractions;

namespace LineToBibleReference.Console;

public class Worker : BackgroundService
{
    private readonly ITextToDataConverter _textToDataConverter;
    private readonly ILogger<Worker> _logger;

    public Worker(ITextToDataConverter textToDataConverter, ILogger<Worker> logger)
    {
        _textToDataConverter = textToDataConverter;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var list = _textToDataConverter.ConvertToBibleReferences().ToBlockingEnumerable(stoppingToken).ToList();

        _logger.LogInformation("count: {count}", list.Count);

        await Task.Delay(0);
    }
}
