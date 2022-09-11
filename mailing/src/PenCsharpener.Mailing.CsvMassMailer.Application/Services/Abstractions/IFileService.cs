using PenCsharpener.Mailing.CsvMassMailer.Application.Models;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

public interface IFileService
{
    Task<string[]> ReadAllLinesOfCsvAsync(CancellationToken cancellationToken = default);
    Task<EmailTemplate> ReadTemplateAsync(CancellationToken cancellationToken = default);
    Task WriteEmailsAsync(EmailText[] emails, CancellationToken cancellationToken = default);
}

