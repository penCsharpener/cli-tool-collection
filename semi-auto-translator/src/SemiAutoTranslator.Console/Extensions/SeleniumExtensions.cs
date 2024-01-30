using OpenQA.Selenium;

namespace SemiAutoTranslator.Console.Extensions;
public static class SeleniumExtensions
{
    public static IWebElement? ToElement(this By by, IWebDriver driver)
    {
        try
        {
            return driver.FindElement(by);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
