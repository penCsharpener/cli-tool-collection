namespace PenCsharpener.Mailing.CsvMassMailer.Application.Models;
public record EmailText
{
    public EmailText(string recipient, string emailBody)
    {
        Recipient = recipient;
        EmailBody = emailBody;
    }

    public string Recipient { get; }
    public string EmailBody { get; }
}
