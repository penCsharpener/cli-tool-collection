using System.IO.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Infrastructure.Tests.Integration;

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
        var fileContent = await _fileService.ReadAllLinesOfCsvAsync();
        fileContent.Should().NotBeNull();

        var csvContent = _csvReader.ParseCsvContent(fileContent, ",");
        var template = await _fileService.ReadTemplateAsync();

        var emails = _testObject.GenerateEmails(template.Body, csvContent);

        await _fileService.WriteEmailsAsync(emails.ToArray());

        template.Subject.Should().Be("email-template");
    }
}
