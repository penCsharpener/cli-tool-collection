namespace PenCsharpener.Extensions.Tests;

public class EnumerableExtensionTests
{
    [Theory]
    [MemberData(nameof(EnumerableData))]
    public void IsNullOrEmpty_Returns_True(IEnumerable<string>? enumerable)
    {
        enumerable.IsNullOrEmpty().Should().BeTrue();
    }

    public static IEnumerable<object[]> EnumerableData()
    {
        yield return new object[] { Enumerable.Empty<string>() };
        yield return new object[] { null! };
    }
}