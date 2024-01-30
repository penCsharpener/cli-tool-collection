using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;
using SemiAutoTranslator.Console.Services;
using Xunit.Abstractions;

namespace SemiAutoTranslator.Console.Tests.Services;

public class GutenbergTextDividerServiceTests
{
    private readonly AppSettings _appSettings;
    private readonly IFileService _fileService;
    private readonly GutenbergTextDividerService _sut;
    private readonly ITestOutputHelper _outputHelper;

    public GutenbergTextDividerServiceTests(ITestOutputHelper outputHelper)
    {
        _appSettings = new AppSettings() { MaxCharactersPerRequest = 200 };
        _fileService = Substitute.For<IFileService>();

        _sut = new GutenbergTextDividerService(_appSettings, _fileService);
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task GetTextSnippet_Produces_Snippets_Below_Limit()
    {
        string[] exampleText = [@"1 Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 2 Ullamcorper a lacus vestibulum sed arcu non odio euismod lacinia. 3 Ut lectus arcu bibendum at varius vel. 4 Cras semper auctor neque vitae tempus quam pellentesque nec. 5 Ultricies leo integer malesuada nunc. 6 Dui vivamus arcu felis bibendum ut. 
7 Ut faucibus pulvinar elementum integer enim neque volutpat! 8 Sollicitudin nibh sit amet commodo nulla facilisi nullam vehicula. 9 Ultricies leo integer malesuada nunc vel risus commodo. 10 Odio facilisis mauris sit amet massa vitae tortor. 11 Adipiscing vitae proin sagittis nisl rhoncus mattis rhoncus. 
12 Eget dolor morbi non arcu risus quis varius quam. 13 Enim facilisis gravida neque convallis a cras semper auctor neque. 14 Nibh nisl condimentum id venenatis a condimentum. 15 Sodales neque sodales ut etiam sit amet nisl? 16 Nec sagittis aliquam malesuada bibendum arcu vitae elementum. 17 Nibh sed pulvinar proin gravida. 
18 Id diam vel quam elementum pulvinar. 19 Imperdiet massa tincidunt nunc pulvinar. 20 Eget duis at tellus at. "];

        _fileService.GetLinesInFile().Returns(exampleText.ToAsyncEnumerable());

        await foreach (var text in _sut.GetTextSnippet())
        {
            _outputHelper.WriteLine($"Length: {text.Length}      {text}");
            text.Length.Should().BeLessThan(_appSettings.MaxCharactersPerRequest);
        }
    }
}
