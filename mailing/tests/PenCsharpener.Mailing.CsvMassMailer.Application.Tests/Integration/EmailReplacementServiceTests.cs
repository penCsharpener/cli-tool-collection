using System.IO.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Tests.Integration;

public class EmailReplacementServiceTests
{
    private readonly IEmailReplacementService _testObject;
    private readonly ICsvReader _csvReader;
    private readonly IFileService _fileService;
    private readonly IFileSystem _fileSystem;

    public EmailReplacementServiceTests(IEmailReplacementService testObject, ICsvReader csvReader, IFileService fileService, IFileSystem fileSystem)
    {
        _testObject = testObject;
        _csvReader = csvReader;
        _fileService = fileService;
        _fileSystem = fileSystem;
    }

    [Fact]
    public async Task Services_Prepare_Email_Bodies_And_Save_Emails_As_Text_Files()
    {
        var fileContent = await _fileSystem.File.ReadAllLinesAsync("Integration/CsvMassMailer.Tests.csv");
        fileContent.Should().NotBeNull();

        var csvContent = _csvReader.ParseCsvContent(fileContent);
        var template = await _fileSystem.File.ReadAllTextAsync("Integration/email-template.txt");

        var emails = _testObject.GenerateEmails(template, csvContent);

        await _fileService.WriteEmailsAsync(emails.ToArray());

        //assert


    }
}
