namespace LineToBibleReference.Console.Models;

public sealed record BibleBook : IComparable<BibleBook>
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ChaptersLxx { get; set; }
    public required int ChaptersHeb { get; set; }
    public string[] AlternativeNames { get; set; } = default!;

    public int CompareTo(BibleBook? other)
    {
        if (other is null)
        {
            return 0;
        }

        return ReferenceValue().CompareTo(other.ReferenceValue());
    }

    public static bool operator >(BibleBook operand1, BibleBook operand2)
    {
        return operand1.CompareTo(operand2) > 0;
    }

    public static bool operator <(BibleBook operand1, BibleBook operand2)
    {
        return operand1.CompareTo(operand2) < 0;
    }

    public static bool operator >=(BibleBook operand1, BibleBook operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }

    public static bool operator <=(BibleBook operand1, BibleBook operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }

    private int ReferenceValue()
    {
        return Id;
    }
}