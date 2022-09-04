using PenCsharpener.Mailing.Common.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;
using PenCsharpener.Mailing.CsvMassMailer.Models;
using PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Services;

public class CsvMassMailerService : ICsvMassMailerService
{
    private readonly IFileService _fileService;
    private readonly ICsvReader _csvReader;
    private readonly IEmailReplacementService _replacementService;
    private readonly ISmtpService _smtpService;

    public CsvMassMailerService(IFileService fileService, ICsvReader csvReader, IEmailReplacementService replacementService, ISmtpService smtpService)
    {
        _fileService = fileService;
        _csvReader = csvReader;
        _replacementService = replacementService;
        _smtpService = smtpService;
    }

    public async Task ExecuteAsync(CoconaOptions options, CancellationToken cancellationToken = default)
    {
        var csvLines = await _fileService.ReadAllLinesOfCsvAsync(cancellationToken);
        var template = await _fileService.ReadTemplateAsync(cancellationToken);

        var csvContent = _csvReader.ParseCsvContent(csvLines, options.CsvDelimiter);
        var emailTexts = _replacementService.GenerateEmails(template, csvContent);

        await _fileService.WriteEmailsAsync(emailTexts.ToArray(), cancellationToken);

        if (options.SendEmails)
        {
            foreach (var mail in emailTexts)
            {
                await _smtpService.SendEmailAsync(mail.Recipient, mail.Recipient, "filename of template file", mail.EmailBody, cancellationToken);
            }
        }
    }
}