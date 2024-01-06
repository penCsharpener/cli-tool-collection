using HtmlAgilityPack;
using TextCompAs.Application.Abstractions;
using TextCompAs.Application.Models;

namespace TextCompAs.Infrastructure.Services;

public sealed class GutenbergTextDownloader : ITextProvider
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _settings;

    public GutenbergTextDownloader(HttpClient httpClient, AppSettings settings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings;
    }

    [Obsolete]
    public async Task<string> GetTextAsync(CancellationToken token = default)
    {
        var text = await _httpClient.GetStringAsync(_settings.SourcePath, token);

        var doc = new HtmlDocument();

        doc.LoadHtml(text);

        var textElements = doc.DocumentNode.DescendantNodes().Select(h => h.InnerText).Where(h => !h.Contains("margin-top") && !h.Contains("pg-header") && !string.IsNullOrEmpty(h) && h != "\r\n");
        var textArray = FilterDuplicateEntries(textElements).ToArray();

        return string.Join("\n", textArray);
    }

    private IEnumerable<string> FilterDuplicateEntries(IEnumerable<string> text)
    {
        var previousText = string.Empty;

        foreach (var h in text)
        {
            if (h == previousText)
            {
                continue;
            }

            previousText = h;
            yield return h;
        }
    }

    //private IEnumerable<string> GetInnerHtmlRecursively()
    //{

    //}

}
