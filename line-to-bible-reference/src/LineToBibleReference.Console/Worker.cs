using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console;

public class Worker : BackgroundService
{
    private readonly IDataConverterFactory _converterFactory;
    private readonly ICharacterStatistics _characterStatistics;
    private readonly IWordStatistics _wordStatistics;
    private readonly IFileService _fileService;
    private readonly IMorphReader _morphReader;
    private readonly IValueGrouper _valueGrouper;
    private readonly IHostApplicationLifetime _host;
    private readonly AppSettings _settings;
    private readonly ILogger<Worker> _logger;

    public Worker(IDataConverterFactory converterFactory,
                  ICharacterStatistics characterStatistics,
                  IWordStatistics wordStatistics,
                  IFileService fileService,
                  IMorphReader morphReader,
                  IValueGrouper valueGrouper,
                  IHostApplicationLifetime host,
                  AppSettings settings,
                  ILogger<Worker> logger)
    {
        _converterFactory = converterFactory;
        _characterStatistics = characterStatistics;
        _wordStatistics = wordStatistics;
        _fileService = fileService;
        _morphReader = morphReader;
        _valueGrouper = valueGrouper;
        _host = host;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        ParseCsvFiles();

        _host.StopApplication();

        await StopAsync(token);
    }

    private void ParseCsvFiles(CancellationToken token = default)
    {
        var items = _morphReader.ReadMorphologyAsync().ToBlockingEnumerable(token).ToList();

        //await _valueGrouper.WriteAllUnitValues(items);
    }

    private async Task IterateOverTexts(CancellationToken token = default)
    {
        Dictionary<string, List<BibleVerseModel>> dict = new();
        Dictionary<string, List<WordStatsItem>> wordStats = new();

        foreach (var converterType in new[] { "esv", "kjv" })
        {
            var list = _converterFactory.GetDataConverter(converterType).ConvertToBibleReferences().ToBlockingEnumerable(token).ToList();
            var wordList = _wordStatistics.GetBibleWordStats(converterType, list);

            wordStats.Add(converterType, wordList.ToList());
            dict.Add(converterType, list);

            var stats = _characterStatistics.GetLanguageStatistics(list);

            _logger.LogInformation("count: {count}", list.Count);
            //list.GroupBy(r => new { r.BookAbbreviation, r.Chapter }).ToList().ForEach(g => _logger.LogInformation("{book} {chapter}", g.Key.BookAbbreviation, g.Key.Chapter));
            await Task.Delay(0);
        }
    }
}
