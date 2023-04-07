namespace LineToBibleReference.Console.Models;

public sealed record BibleReference
{
    public required int BookId { get; set; }
    public required string Name { get; init; }
    public string? Description { get; set; }
    public required int Chapter { get; set; }
    public required int Verse { get; set; }
}
