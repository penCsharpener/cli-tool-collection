using System.Runtime.CompilerServices;
using OpenQA.Selenium;

namespace SemiAutoTranslator.Console.Abstractions;

public interface IWebDriverFactory
{
    By GetSelectorFromProperty(string value, [CallerArgumentExpression("value")] string propertyName = "");
    Task<IWebDriver> GetWebDriver(CancellationToken token = default);
}