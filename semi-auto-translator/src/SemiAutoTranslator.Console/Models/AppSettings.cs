namespace SemiAutoTranslator.Console.Models;
public class AppSettings
{
    public string Url { get; set; } = default!;
    public string XPathCookieBannerButton { get; set; } = default!;
    public string CssSelectorPluginButton { get; set; } = default!;
    public string CssSelectorSourceInput { get; set; } = default!;
    public string CssSelectorTranslatedTextElements { get; set; } = default!;
    public int MaxCharactersPerRequest { get; set; }
    public TextSource TextSource { get; set; } = default!;
}

public class TextSource
{
    public string FilePath { get; set; } = default!;
}