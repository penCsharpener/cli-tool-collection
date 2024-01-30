using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;

namespace SemiAutoTranslator.Console.Services;

public class FirefoxWebDriverFactory : IWebDriverFactory
{
    private readonly AppSettings _settings;

    public FirefoxWebDriverFactory(AppSettings settings)
    {
        _settings = settings;
    }

    public async Task<IWebDriver> GetWebDriver(CancellationToken token = default)
    {
        var driver = new FirefoxDriver
        {
            Url = _settings.Url
        };
        await Task.Delay(2000, token);

        var cookieBannerButton = driver.FindElement(GetSelectorFromProperty(_settings.XPathCookieBannerButton));
        cookieBannerButton.Click();
        await Task.Delay(500, token);

        IWebElement? firefoxPluginButton = null;

        while (firefoxPluginButton == null)
        {
            await Task.Delay(500, token);

            firefoxPluginButton = driver.FindElement(GetSelectorFromProperty(_settings.CssSelectorPluginButton));
        }

        firefoxPluginButton?.Click();
        await Task.Delay(500, token);

        var sourceInput = driver.FindElement(GetSelectorFromProperty(_settings.CssSelectorSourceInput));
        var translatedTextElements = driver.FindElements(GetSelectorFromProperty(_settings.CssSelectorTranslatedTextElements));

        return driver;
    }

    public By GetSelectorFromProperty(string value, [CallerArgumentExpression(nameof(value))] string propertyName = "")
    {
        if (propertyName.Contains(".CssSelector"))
        {
            return By.CssSelector(value);
        }
        else if (propertyName.Contains(".XPath"))
        {
            return By.XPath(value);
        }

        throw new UnreachableException();
    }
}
