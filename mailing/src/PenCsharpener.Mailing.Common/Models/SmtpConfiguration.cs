namespace PenCsharpener.Mailing.Common.Models;

public class SmtpConfiguration
{
    public string SmtpHost { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = default!;
    public string SmtpSenderName { get; set; } = default!;
    public string SmtpSenderAddress { get; set; } = default!;
    public string SmtpPassword { get; set; } = default!;
}
