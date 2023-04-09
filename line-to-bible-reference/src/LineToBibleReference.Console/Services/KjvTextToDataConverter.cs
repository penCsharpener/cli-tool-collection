using System.Text.RegularExpressions;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class KjvTextToDataConverter : ITextToDataConverter
{
    private readonly IFileService _fileService;
    private readonly AppSettings _settings;
    private static readonly Regex _regex = new(@"(\s{11})(\d?\s?\w*\s)?(\d*:)?(\d+)(\s{7})(.*)");
    private static readonly Regex _alternativeLines = new(@"(\s{11})(\d?\s?\w*\s)?(\d*:)?(\w{5})(\s{7})(.*)");

    public KjvTextToDataConverter(IFileService fileService, AppSettings settings)
    {
        _fileService = fileService;
        _settings = settings;
    }

    public async IAsyncEnumerable<BibleVerseModel> ConvertToBibleReferences()
    {
        var bookName = string.Empty;
        BibleBook bibleBook = default!;
        BibleVerseModel bibleVerseModel = default!;
        ParsedReference parsedReference = default!;

        await foreach (var line in _fileService.ReadByLineAsync(_settings.ConverterPathMapping["kjv"]))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            parsedReference = ParseLine(line, parsedReference);

            if (parsedReference.BelongsToPreviewsLine)
            {
                bibleVerseModel.Text += parsedReference.Text;

                continue;
            }

            if (bibleVerseModel is not null)
            {
                yield return bibleVerseModel;
            }

            if (bookName != parsedReference.BookName)
            {
                bibleBook = Constants.BibleBooks.First(bb => bb.AlternativeNames.Contains(parsedReference.BookName) || bb.Name == parsedReference.BookName);
            }

            var words = parsedReference.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var bibleReference = new BibleReference
            {
                BookId = bibleBook.Id,
                Chapter = parsedReference.Chapter,
                Name = bibleBook.Name,
                Verse = parsedReference.Verse,
            };

            bibleVerseModel = new BibleVerseModel
            {
                BookAbbreviation = parsedReference.BookName,
                BibleReference = bibleReference,
                Chapter = parsedReference.Chapter,
                Verse = parsedReference.Verse,
                Text = parsedReference.Text,
                Words = words
            };

            bookName = parsedReference.BookName;
        }

        yield return bibleVerseModel;
    }

    private ParsedReference ParseLine(string line, ParsedReference parsedReference)
    {
        var match = _regex.Match(line);

        if (match.Success)
        {
            return new(string.IsNullOrWhiteSpace(match.Groups[2].Value) ? parsedReference.BookName : match.Groups[2].Value.Trim(),
                string.IsNullOrWhiteSpace(match.Groups[3].Value) ? parsedReference.Chapter : int.Parse(match.Groups[3].Value.Trim(':')),
                string.IsNullOrWhiteSpace(match.Groups[4].Value) ? parsedReference.Verse : int.Parse(match.Groups[4].Value),
                match.Groups[6].Value.Trim() + " ",
                false);
        }

        match = _alternativeLines.Match(line);

        if (match.Success)
        {
            return parsedReference with
            {
                Chapter = int.TryParse(match.Groups[3].Value.Trim(':'), out var chapter) ? chapter : parsedReference.Chapter,
                Verse = 0,
                Text = match.Groups[6].Value.Trim() + " ",
                BelongsToPreviewsLine = false
            };
        }

        return parsedReference with
        {
            Text = line.Trim() + " ",
            BelongsToPreviewsLine = true
        };
    }

    private record ParsedReference(string BookName, int Chapter, int Verse, string Text, bool BelongsToPreviewsLine);
}
