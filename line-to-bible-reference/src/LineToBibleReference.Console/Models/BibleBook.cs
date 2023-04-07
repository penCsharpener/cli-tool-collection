namespace LineToBibleReference.Console.Models;

public sealed record BibleBook
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ChaptersLxx { get; set; }
    public required int ChaptersHeb { get; set; }
    public string[] AlternativeNames { get; set; } = default!;
}