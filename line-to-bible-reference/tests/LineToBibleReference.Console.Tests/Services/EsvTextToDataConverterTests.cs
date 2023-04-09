using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;
using LineToBibleReference.Console.Services;

namespace LineToBibleReference.Console.Tests.Services;

public class EsvTextToDataConverterTests
{
    private readonly IFileService _fileService;
    private readonly AppSettings _settings;
    private readonly EsvTextToDataConverter _testObject;

    public EsvTextToDataConverterTests()
    {
        _fileService = Substitute.For<IFileService>();
        _settings = new()
        {
            ConverterPathMapping = new() { { "esv", "path" } }
        };

        _testObject = new(_fileService, _settings);
    }

    [Fact]
    public void Concatenates_Multiple_Lines_To_Single_Verse()
    {
        _fileService.ReadByLineAsync(Arg.Any<string>()).Returns(LoremIpsumGenerator());

        var result = _testObject.ConvertToBibleReferences().ToBlockingEnumerable().ToList();

        result.Count.Should().Be(4);
    }

    private async IAsyncEnumerable<string> LoremIpsumGenerator()
    {
        yield return "           Ge 1:1       Lorem ipsum dolor sit amet, ";
        yield return "           2       consectetur adipiscing elit, ";
        yield return "";
        yield return "      sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";
        yield return "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ";
        yield return "";
        yield return "           3       Excepteur sint occaecat cupidatat non proident, ";
        yield return "           4:title       sunt in culpa qui officia deserunt mollit anim id est laborum.";

        await Task.Delay(0);
    }
}
