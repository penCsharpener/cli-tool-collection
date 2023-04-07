namespace LineToBibleReference.Console.Models;

public sealed class BibleVerseModel
{
    public string BookAbbreviation { get; set; } = default!;
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public required BibleReference BibleReference { get; set; }
    public string[]? Words { get; set; }
    public string Text { get; set; } = default!;

    public override string ToString()
    {
        return $"{BookAbbreviation} {Chapter}:{Verse} {Text}";
    }
}

