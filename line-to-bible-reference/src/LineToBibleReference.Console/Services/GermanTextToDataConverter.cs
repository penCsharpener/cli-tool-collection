using System.Text.RegularExpressions;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class GermanTextToDataConverter : ITextToDataConverter
{
    private readonly IFileService _fileService;
    private readonly AppSettings _settings;
    private static readonly Regex _regex = new(@"(\s{11})(\d?\s?\w*\s)?(\d*:)?(\d+)(\s{7})(.*)");

    public GermanTextToDataConverter(IFileService fileService, AppSettings settings)
    {
        _fileService = fileService;
        _settings = settings;
    }

    public async IAsyncEnumerable<BibleReferenceModel> ConvertToBibleReferences()
    {
        var bookName = string.Empty;
        var chapter = 1;
        var verse = 1;

        await foreach (var line in _fileService.ReadByLineAsync(_settings.PathToGermanTextFile))
        {
            var match = _regex.Match(line);
            bookName = string.IsNullOrWhiteSpace(match.Groups[2].Value) ? bookName : match.Groups[2].Value;
            chapter = string.IsNullOrWhiteSpace(match.Groups[3].Value) ? chapter : int.Parse(match.Groups[3].Value.Trim(':'));
            verse = string.IsNullOrWhiteSpace(match.Groups[4].Value) ? verse : int.Parse(match.Groups[4].Value);
            var text = match.Groups[6].Value;

            yield return new BibleReferenceModel
            {
                BookAbbreviation = bookName.Trim(),
                Chapter = chapter,
                Verse = verse,
                Text = text
            };
        }
    }
}
