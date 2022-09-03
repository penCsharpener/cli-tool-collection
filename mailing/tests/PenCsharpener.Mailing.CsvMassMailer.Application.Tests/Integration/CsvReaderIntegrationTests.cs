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

        csvContent.Should().NotBeNull();
    }
}
