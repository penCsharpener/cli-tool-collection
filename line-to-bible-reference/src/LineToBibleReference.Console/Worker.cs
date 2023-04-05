using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console;

public class Worker : BackgroundService
{
    private readonly IDataConverterFactory _converterFactory;
    private readonly ICharacterStatistics _characterStatistics;
    private readonly IFileService _fileService;
    private readonly IHostApplicationLifetime _host;
    private readonly AppSettings _settings;
    private readonly ILogger<Worker> _logger;

    public Worker(IDataConverterFactory converterFactory, ICharacterStatistics characterStatistics, IFileService fileService, IHostApplicationLifetime host, AppSettings settings, ILogger<Worker> logger)
    {
        _converterFactory = converterFactory;
        _characterStatistics = characterStatistics;
        _fileService = fileService;
        _host = host;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Dictionary<string, List<BibleReferenceModel>> dict = new();

        foreach (var converterType in new[] { "de", "heb", "gr" })
        {
            var list = _converterFactory.GetDataConverter(converterType).ConvertToBibleReferences().ToBlockingEnumerable(stoppingToken).ToList();
            dict.Add(converterType, list);

            var stats = _characterStatistics.GetLanguageStatistics(list);

            _logger.LogInformation("count: {count}", list.Count);
            list.GroupBy(r => new { r.BookAbbreviation, r.Chapter }).ToList().ForEach(g => _logger.LogInformation("{book} {chapter}", g.Key.BookAbbreviation, g.Key.Chapter));
        }

        _host.StopApplication();

        await StopAsync(stoppingToken);
    }
}
