using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Models;
using PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Services;

public class CsvMassMailerService : ICsvMassMailerService
{
    private readonly IFileService _fileService;
    private readonly ICsvReader _csvReader;
    private readonly IEmailReplacementService _replacementService;

    public CsvMassMailerService(IFileService fileService, ICsvReader csvReader, IEmailReplacementService replacementService)
    {
        _fileService = fileService;
        _csvReader = csvReader;
        _replacementService = replacementService;
    }

    public async Task ExecuteAsync(CoconaOptions options, CancellationToken cancellationToken = default)
    {
        var csvLines = await _fileService.ReadAllLinesOfCsvAsync();
        var template = await _fileService.ReadTemplateAsync();

        var csvContent = _csvReader.ParseCsvContent(csvLines);
        var emailTexts = _replacementService.GenerateEmails(template, csvContent);

        await _fileService.WriteEmailsAsync(emailTexts.ToArray());
    }
}