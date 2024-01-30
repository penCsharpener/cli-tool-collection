using System.Collections.ObjectModel;
using OpenQA.Selenium;
using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Extensions;
using SemiAutoTranslator.Console.Models;

namespace SemiAutoTranslator.Console.Services;

public class GutenbergTranslator : ITranslator, IDisposable
{
    private readonly AppSettings _settings;
    private readonly IWebDriverFactory _driverFactory;
    private readonly ITextDividerService _gutenbergTextDivider;
    private readonly IFileService _fileService;
    private readonly ILogger<GutenbergTranslator> _logger;
    private readonly string _newFileName;
    private IWebDriver? _driver;
    private IWebElement? _sourceInput;
    private ReadOnlyCollection<IWebElement>? _translatedTextElements;

    public GutenbergTranslator(AppSettings appSettings,
                               IWebDriverFactory driverFactory,
                               [FromKeyedServices("Gutenberg")] ITextDividerService textDivider,
                               IFileService fileService,
                               ILogger<GutenbergTranslator> logger)
    {
        _settings = appSettings;
        _driverFactory = driverFactory;
        _gutenbergTextDivider = textDivider;
        _fileService = fileService;
        _logger = logger;
        _newFileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{Path.GetFileNameWithoutExtension(_settings.TextSource.FilePath)}.txt";
    }

    public async Task TranslateAsync(CancellationToken token = default)
    {
        await SetupDriver(token);

        await foreach (var snippet in _gutenbergTextDivider.GetTextSnippet(token))
        {
            if (token.IsCancellationRequested)
            {
                break;
            }

            var usedPage = await TranslateSnippetAsync(snippet, token);
            if (usedPage == false)
            {
                _driver?.Dispose();
                await SetupDriver(token);
                await TranslateSnippetAsync(snippet, token);
            }
        }

        await Task.Delay(10000, token);

        _driver!.Close();
        _driver.Quit();
    }

    private async Task<bool> TranslateSnippetAsync(string snippet, CancellationToken token = default)
    {
        try
        {
            await Task.Delay(500, token);

            _sourceInput!.Clear();
            _sourceInput.SendKeys(snippet);
            await Task.Delay(2000, token);

            var translatedText = default(string);

            while (string.IsNullOrWhiteSpace(translatedText) || translatedText.Contains("[...]  [...]  [...]"))
            {
                Thread.Sleep(500);

                translatedText = string.Join(" ", _translatedTextElements!.Select(e => e.Text).Where(t => !string.IsNullOrWhiteSpace(t)));
            }

            await _fileService.AppendToFile(_newFileName, translatedText + Environment.NewLine + Environment.NewLine, token);

            _logger.LogInformation("end of source: {s} | end of translation: {t}", snippet.GetLast(30), translatedText.GetLast(30));

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Nag-screen popped up: {exc}", ex);
            await _fileService.AppendToFile(_newFileName, Environment.NewLine + Environment.NewLine, token);
            return false;
        }
    }

    private async Task SetupDriver(CancellationToken token = default)
    {
        _driver = await _driverFactory.GetWebDriver(token);

        _sourceInput = _driver.FindElement(_driverFactory.GetSelectorFromProperty(_settings.CssSelectorSourceInput));
        _translatedTextElements = _driver.FindElements(_driverFactory.GetSelectorFromProperty(_settings.CssSelectorTranslatedTextElements));
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
