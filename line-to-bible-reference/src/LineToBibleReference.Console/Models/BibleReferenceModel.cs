namespace LineToBibleReference.Console.Models;

public class BibleReferenceModel
{
    public string BookAbbreviation { get; set; } = default!;
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public string[]? Words { get; set; }
    public string Text { get; set; } = default!;

    public override string ToString()
    {
        return $"{BookAbbreviation} {Chapter}:{Verse} {Text}";
    }
}
