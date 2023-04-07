namespace LineToBibleReference.Console.Models;

public sealed class WordStatsItem
{
    public required string Word { get; set; }
    public int Count { get; set; }
    public IList<BibleReference> BibleReferences { get; set; } = new List<BibleReference>();

    public override string ToString()
    {
        return $"{Word} {Count}";
    }
}