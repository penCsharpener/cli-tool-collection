using PenCsharpener.Mailing.CsvMassMailer.Application.Models;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services;

public class EmailReplacementService : IEmailReplacementService
{
    public IEnumerable<EmailText> GenerateEmails(string template, CsvContent content)
    {
        foreach (var recipient in content.EmailRecipients)
        {
            var emailBody = template;

            foreach (var replacementPair in content.ReplacementPairs[recipient])
            {
                emailBody = emailBody.Replace(replacementPair.Token, replacementPair.Value);
            }

            yield return new(recipient, emailBody);
        }
    }
}
