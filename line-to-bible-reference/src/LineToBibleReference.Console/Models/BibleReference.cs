using System.Text.RegularExpressions;

namespace LineToBibleReference.Console.Models;

public sealed record BibleReference : IComparable<BibleReference>
{
    private static readonly Regex _regex = new("(\\w.*?)(\\s)(\\d*?)(:)(\\d*)");
    private static readonly Regex _regexWithoutChapter = new("(\\w.*?)(\\s)(\\d*)");

    public required int BookId { get; set; }
    public required string Name { get; init; }
    public string? Description { get; set; }
    public required int Chapter { get; set; }
    public required int Verse { get; set; }
    public int VerseId => ReferenceValue();

    public int CompareTo(BibleReference? other)
    {
        if (other is null)
        {
            return 0;
        }

        return ReferenceValue().CompareTo(other.ReferenceValue());
    }

    public static bool operator >(BibleReference operand1, BibleReference operand2)
    {
        return operand1.CompareTo(operand2) > 0;
    }

    public static bool operator <(BibleReference operand1, BibleReference operand2)
    {
        return operand1.CompareTo(operand2) < 0;
    }

    public static bool operator >=(BibleReference operand1, BibleReference operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <=(BibleReference operand1, BibleReference operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }

    private int ReferenceValue()
    {
        return Verse + (Chapter * 1000) + (BookId * 1000000);
    }

    public static BibleReference Parse(string value)
    {


        var match = value.Contains(':') ? _regex.Match(value) : _regexWithoutChapter.Match(value);

        var bookName = match.Groups[1].Value;
        var book = Constants.BibleBooks.Single(b => b.Name == bookName || b.AlternativeNames.Contains(bookName));

        var (chapter, verse) = GetChapterVerse(match);

        var reference = new BibleReference()
        {
            Name = bookName,
            BookId = book.Id,
            Chapter = chapter,
            Verse = verse
        };

        return reference;
    }

    private static (int Chapter, int Verse) GetChapterVerse(Match match)
    {
        if (string.IsNullOrWhiteSpace(match.Groups[5].Value))
        {
            return (1, int.Parse(match.Groups[3].Value));
        }

        return (int.Parse(match.Groups[3].Value), int.Parse(match.Groups[5].Value));
    }
}
