using System.IO.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests.Integration;

public class CsvReaderIntegrationTests
{
    private readonly IFileSystem _fileSystem;
    private readonly ICsvReader _csvReader;

    public CsvReaderIntegrationTests(IFileSystem file, ICsvReader csvReader)
    {
        _fileSystem = file;
        _csvReader = csvReader;
    }

    [Fact]
    public async Task ParseCsvContent_Parses_Csv_Correctly()
    {
        var fileContent = await _fileSystem.File.ReadAllLinesAsync("Integration/CsvMassMailer.Tests.csv");

        fileContent.Should().NotBeNull();

        var csvContent = _csvReader.ParseCsvContent(fileContent);

        const string johnDoe = "john.doe@example.com";
        const string babyDoe = "baby.doe@example.com";
        csvContent.Should().NotBeNull();
        csvContent.EmailRecipients![0].Should().Be(johnDoe);
        csvContent.ReplacementTokens![johnDoe][0].Token.Should().Be("{{Name}}");
        csvContent.ReplacementTokens![johnDoe][0].Value.Should().Be("John Doe");
        csvContent.ReplacementTokens![johnDoe][1].Token.Should().Be("{{DoB}}");
        csvContent.ReplacementTokens![johnDoe][1].Value.Should().Be("01.01.1980");
        csvContent.ReplacementTokens![johnDoe][2].Token.Should().Be("{{registration fee}}");
        csvContent.ReplacementTokens![johnDoe][2].Value.Should().Be("90");
        csvContent.ReplacementTokens![johnDoe][3].Token.Should().Be("{{currency}}");
        csvContent.ReplacementTokens![johnDoe][3].Value.Should().Be("€");
        csvContent.EmailRecipients[1].Should().Be("jane.doe@example.com");
        csvContent.EmailRecipients[2].Should().Be("terrence.doe@example.com");
        csvContent.EmailRecipients[3].Should().Be(babyDoe);
        csvContent.ReplacementTokens![babyDoe][0].Token.Should().Be("{{Name}}");
        csvContent.ReplacementTokens![babyDoe][0].Value.Should().Be("Baby Doe");
        csvContent.ReplacementTokens![babyDoe][1].Token.Should().Be("{{DoB}}");
        csvContent.ReplacementTokens![babyDoe][1].Value.Should().Be("05.08.2022");
        csvContent.ReplacementTokens![babyDoe][2].Token.Should().Be("{{registration fee}}");
        csvContent.ReplacementTokens![babyDoe][2].Value.Should().Be("0");
        csvContent.ReplacementTokens![babyDoe][3].Token.Should().Be("{{currency}}");
        csvContent.ReplacementTokens![babyDoe][3].Value.Should().Be("");
    }
}
