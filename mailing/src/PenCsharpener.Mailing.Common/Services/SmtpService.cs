using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PenCsharpener.Mailing.Common.Models;
using PenCsharpener.Mailing.Common.Services.Abstractions;

namespace PenCsharpener.Mailing.Common.Services;

public class SmtpService : ISmtpService
{
    private readonly SmtpConfiguration _options;
    private readonly ILogger<SmtpService> _logger;

    public SmtpService(IOptions<SmtpConfiguration> options, ILogger<SmtpService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string recipientName, string recipientAddress, string subject, string emailBody, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SmtpSenderName, _options.SmtpSenderAddress));
        message.To.Add(new MailboxAddress(recipientName, recipientAddress));
        message.Subject = subject;

        message.Body = CreateEmailBody(emailBody);

        using var client = new SmtpClient();
        await client.ConnectAsync(_options.SmtpHost, _options.SmtpPort, MailKit.Security.SecureSocketOptions.Auto, cancellationToken);

        // Note: since we don't have an OAuth2 token, disable
        // the XOAUTH2 authentication mechanism.
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        // Note: only needed if the SMTP server requires authentication
        await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword, cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    private MimeEntity CreateEmailBody(string emailBody)
    {
        if (emailBody.Contains('<') && emailBody.Contains('>'))
        {
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody
            };

            return bodyBuilder.ToMessageBody();
        }

        return new TextPart(TextFormat.Plain)
        {
            Text = emailBody
        };
    }
}

public class NullSmtpService : ISmtpService
{
    public Task SendEmailAsync(string recipientName, string recipientAddress, string subject, string emailBody, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}