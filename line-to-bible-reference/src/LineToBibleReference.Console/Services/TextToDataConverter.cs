using System.Text.RegularExpressions;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class TextToDataConverter : ITextToDataConverter
{
    private readonly IFileService _fileService;
    private static readonly Regex _lineMatch = new(@"(\s{11})(\d?\s?\w*\s)?(\d*:)?(\d+)(\s{7})(.*)");

    public TextToDataConverter(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async IAsyncEnumerable<BibleReferenceModel> ConvertToBibleReferences()
    {
        var bookName = string.Empty;
        var chapter = 1;
        var verse = 1;

        await foreach (var line in _fileService.ReadByLineAsync())
        {
            var regex = _lineMatch.Match(line);
            bookName = string.IsNullOrWhiteSpace(regex.Groups[2].Value) ? bookName : regex.Groups[2].Value;
            chapter = string.IsNullOrWhiteSpace(regex.Groups[3].Value) ? chapter : int.Parse(regex.Groups[3].Value.Trim(':'));
            verse = string.IsNullOrWhiteSpace(regex.Groups[4].Value) ? verse : int.Parse(regex.Groups[4].Value);
            var text = regex.Groups[6].Value;

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
