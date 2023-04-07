using System.Text.RegularExpressions;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class HebrewTextToDataConverter : ITextToDataConverter
{
    private readonly IFileService _fileService;
    private readonly AppSettings _settings;
    private static readonly Regex _regex = new(@"(\d+\s)?(\d+)(\s{7})(.*)");

    public HebrewTextToDataConverter(IFileService fileService, AppSettings settings)
    {
        _fileService = fileService;
        _settings = settings;
    }

    public async IAsyncEnumerable<BibleVerseModel> ConvertToBibleReferences()
    {
        var bookName = string.Empty;
        var chapter = 1;
        var verse = 1;

        await foreach (var line in _fileService.ReadByLineAsync(_settings.PathToHebrewTextFile))
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.Contains("       "))
            {
                bookName = line.Trim();

                continue;
            }

            var match = _regex.Match(line);
            chapter = string.IsNullOrWhiteSpace(match.Groups[1].Value) ? chapter : int.Parse(match.Groups[1].Value.Trim());
            verse = string.IsNullOrWhiteSpace(match.Groups[2].Value) ? verse : int.Parse(match.Groups[2].Value);
            var text = match.Groups[4].Value;
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var bibleBook = Constants.BibleBooks.First(bb => bb.AlternativeNames.Contains(bookName) || bb.Name == bookName);
            var bibleReference = new BibleReference
            {
                BookId = bibleBook.Id,
                Chapter = chapter,
                Name = bibleBook.Name,
                Verse = verse
            };

            yield return new BibleVerseModel
            {
                BookAbbreviation = bookName,
                BibleReference = bibleReference,
                Chapter = chapter,
                Verse = verse,
                Text = text,
                Words = words
            };
        }
    }
}
