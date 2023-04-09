using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Tests.Models;
public class BibleReferenceTests
{
    [Fact]
    private void BibleReference_Sorts_Correctly()
    {
        var references = new BibleReference[]
        {
            new() { Name = "Dt", BookId = 5, Chapter = 3, Verse = 6 },
            new() { Name = "Mt", BookId = 40, Chapter = 5, Verse = 12 },
            new() { Name = "Dt", BookId = 33, Chapter = 14, Verse = 7 },
            new() { Name = "Ru", BookId = 8, Chapter = 3, Verse = 6 },
            new() { Name = "Rev", BookId =66, Chapter = 22, Verse = 10 },
            new() { Name = "Isa", BookId = 23, Chapter = 40, Verse = 3 },
            new() { Name = "Isa", BookId = 23, Chapter = 40, Verse = 6 },
        };

        var sorted = references.OrderBy(x => x).ToList();
    }
}
