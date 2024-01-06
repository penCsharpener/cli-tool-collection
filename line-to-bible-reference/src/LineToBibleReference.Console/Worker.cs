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
    private readonly IRootGroupingService _rootGroupingService;
    private readonly IHostApplicationLifetime _host;
    private readonly AppSettings _settings;
    private readonly ILogger<Worker> _logger;

    public Worker(IDataConverterFactory converterFactory,
                  ICharacterStatistics characterStatistics,
                  IWordStatistics wordStatistics,
                  IFileService fileService,
                  IMorphReader morphReader,
                  IValueGrouper valueGrouper,
                  IRootGroupingService rootGroupingService,
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
        _rootGroupingService = rootGroupingService;
        _host = host;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token = default)
    {
        await ParseCsvFiles(token);

        //await IterateOverTexts(token);

        _host.StopApplication();

        await StopAsync(token);
    }

    private async Task ParseCsvFiles(CancellationToken token = default)
    {
        foreach (var csvPathMapping in _settings.CsvPathMapping)
        {
            var items = _morphReader.ReadMorphologyAsync(csvPathMapping.Key, token).ToBlockingEnumerable(token).ToList();
            items = items.OrderBy(i => i.Reference).ToList();

            _logger.LogInformation("Read {count} items.", items.Count);

            await _fileService.WriteCsvAsync(items, csvPathMapping.Value.TargetPath, csvPathMapping.Value.TargetFileName, token);

            var dict = _rootGroupingService.GroupByRoot(items);

            await _fileService.WriteConcordanceAsync(dict, csvPathMapping.Value.TargetPathRootGrouping, csvPathMapping.Value.TargetFileNameRootGrouping, token);
        }

        //await _valueGrouper.WriteAllUnitValues(items);
    }

    private async Task IterateOverTexts(CancellationToken token = default)
    {
        Dictionary<string, List<BibleVerseModel>> dict = new();
        Dictionary<string, List<WordStatsItem>> wordStats = new();

        foreach (var converterType in _settings.ConverterPathMapping)
        {
            var converterName = converterType.Key;
            var list = _converterFactory.GetDataConverter(converterName).ConvertToBibleReferences().ToBlockingEnumerable(token).ToList();
            var wordList = _wordStatistics.GetBibleWordStats(converterName, list);

            wordStats.Add(converterName, wordList.ToList());
            dict.Add(converterName, list);

            var stats = _characterStatistics.GetLanguageStatistics(list);

            _logger.LogInformation("{type}: count: {count}", converterType, list.Count);
            _logger.LogInformation("{type}: word count: {wordCount}", converterType, list.Sum(v => v.Words?.Length ?? 0));
            //list.GroupBy(r => new { r.BookAbbreviation, r.Chapter }).ToList().ForEach(g => _logger.LogInformation("{book} {chapter}", g.Key.BookAbbreviation, g.Key.Chapter));
            await Task.Delay(0, token);
        }
    }
}
