namespace LineToBibleReference.Console.Models;

public sealed record BibleReference : IComparable<BibleReference>
{
    public required int BookId { get; set; }
    public required string Name { get; init; }
    public string? Description { get; set; }
    public required int Chapter { get; set; }
    public required int Verse { get; set; }

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
}
