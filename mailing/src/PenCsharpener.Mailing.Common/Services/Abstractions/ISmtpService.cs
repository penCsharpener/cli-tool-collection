namespace PenCsharpener.Mailing.Common.Services.Abstractions;

public interface ISmtpService
{
    Task SendEmailAsync(string recipientName, string recipientAddress, string subject, string emailBody, CancellationToken cancellationToken = default);
}