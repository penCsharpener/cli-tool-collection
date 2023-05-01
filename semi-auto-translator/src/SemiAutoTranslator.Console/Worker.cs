using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SemiAutoTranslator.Console.Models;

namespace SemiAutoTranslator.Console;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly AppSettings _settings;
    private readonly ILogger<Worker> _logger;

    public Worker(IHostApplicationLifetime lifetime, AppSettings settings, ILogger<Worker> logger)
    {
        _lifetime = lifetime;
        _settings = settings;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IWebDriver driver = new FirefoxDriver();

        driver.Url = _settings.Url;

        var cookieBannerButton = driver.FindElement(By.XPath(_settings.XPathCookieBannerButton));
        cookieBannerButton.Click();

        var langSourceButton = driver.FindElement(By.XPath(_settings.XPathLangSourceButton));
        langSourceButton.Click();
        var sourceButton = driver.FindElement(By.XPath(_settings.XPathSourceButton));
        sourceButton.Click();

        var langTargetButton = driver.FindElement(By.XPath(_settings.XPathLangTargetButton));
        langTargetButton.Click();
        var targetButton = driver.FindElement(By.XPath(_settings.XPathTargetButton));
        targetButton.Click();

        var sourceInput = driver.FindElement(By.XPath(_settings.XPathSourceInput));

        await Task.Delay(500);

        sourceInput.SendKeys(_settings.SourceText);

        await Task.Delay(2000);

        var translatedTextElements = driver.FindElements(By.XPath(_settings.XPathTranslatedTextElements));

        var translatedText = default(string);

        while (string.IsNullOrWhiteSpace(translatedText))
        {
            Thread.Sleep(500);

            translatedText = string.Join(" ", translatedTextElements.Select(e => e.Text).Where(t => !string.IsNullOrWhiteSpace(t)));
        }

        _logger.LogInformation("Translated text: {translation}", translatedText);

        await Task.Delay(10000);

        driver.Close();
        driver.Quit();

        _lifetime.StopApplication();

        await Task.Delay(0);
    }
}
