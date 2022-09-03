using PenCsharpener.Mailing.CsvMassMailer.Application.Models;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

public interface IEmailReplacementService
{
    IEnumerable<EmailText> GenerateEmails(string template, CsvContent content);
}
