namespace SemiAutoTranslator.Console.Models;
public class AppSettings
{
    public string Url { get; set; } = default!;
    public string XPathCookieBannerButton { get; set; } = default!;
    public string XPathLangSourceButton { get; set; } = default!;
    public string XPathSourceButton { get; set; } = default!;
    public string XPathLangTargetButton { get; set; } = default!;
    public string XPathTargetButton { get; set; } = default!;
    public string XPathSourceInput { get; set; } = default!;
    public string XPathTranslatedTextElements { get; set; } = default!;
    public string SourceText { get; set; } = default!;
}
