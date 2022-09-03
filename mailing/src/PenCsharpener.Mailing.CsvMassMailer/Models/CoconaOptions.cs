using Cocona;

namespace PenCsharpener.Mailing.CsvMassMailer.Models;
public class CoconaOptions : ICommandParameterSet
{
    [Option("send", Description = "Additionally to printing the email text also send them out directly using smtp settings.")]
    public bool SendEmails { get; set; }

    [Option("host", Description = "Smtp server host.")]
    public bool SmtpHost { get; set; }

    [Option("port", Description = "Smtp server port.")]
    public int SmtpPort { get; set; }

    [Option("username", Description = "Smtp server username.")]
    public bool SmtpUsername { get; set; }

    [Option("password", Description = "Smtp server password.")]
    public bool SmtpPassword { get; set; }
}
