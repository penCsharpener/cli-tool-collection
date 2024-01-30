using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;
using SemiAutoTranslator.Console.Services;
using Xunit.Abstractions;

namespace SemiAutoTranslator.Console.Tests.Services;

public class GutenbergTextDividerServiceIntegrationTests
{
    private readonly AppSettings _appSettings;
    private readonly IFileService _fileService;
    private readonly GutenbergTextDividerService _sut;
    private readonly ITestOutputHelper _outputHelper;

    public GutenbergTextDividerServiceIntegrationTests(ITestOutputHelper outputHelper)
    {
        _appSettings = new AppSettings()
        {
            MaxCharactersPerRequest = 1500,
            TextSource = new TextSource()
            {
                FilePath = ""
            }
        };
        _fileService = new TextFileService(_appSettings);

        _sut = new GutenbergTextDividerService(_appSettings, _fileService);
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task GetTextSnippet_Produces_Snippets_Below_Limit()
    {
        await foreach (var text in _sut.GetTextSnippet())
        {
            if (text.Length > _appSettings.MaxCharactersPerRequest)
            {
                _outputHelper.WriteLine(text);
            }

            text.Length.Should().BeLessThanOrEqualTo(_appSettings.MaxCharactersPerRequest);
        }
    }
}