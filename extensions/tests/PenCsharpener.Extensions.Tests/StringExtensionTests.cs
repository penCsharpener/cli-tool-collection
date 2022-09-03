namespace PenCsharpener.Extensions.Tests;

public class StringExtensionTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void IsNullOrWhiteSpace_Returns_True(string? testString)
    {
        testString.IsNullOrWhiteSpace().Should().BeTrue();
    }
}
