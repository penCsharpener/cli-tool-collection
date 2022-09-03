using System.IO.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests.Integration;

public class CsvReaderIntegrationTests
{
    private readonly IFileSystem _fileSystem;

    public CsvReaderIntegrationTests(IFileSystem file)
    {
        _fileSystem = file;
    }

    [Fact]
    public async Task Test1()
    {
        var fileContent = await _fileSystem.File.ReadAllTextAsync("Integration/CsvMassMailer.Tests.csv");

        fileContent.Should().NotBeNull();
    }
}
