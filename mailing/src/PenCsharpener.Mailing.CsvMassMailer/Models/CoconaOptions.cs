using Cocona;

namespace PenCsharpener.Mailing.CsvMassMailer.Models;
public class CoconaOptions : ICommandParameterSet
{
    [Option("send", Description = "Additionally to printing the email text also send them out directly using smtp settings.")]
    public bool SendEmails { get; set; }

    [Option("d", Description = "Character used to separate columns in csv file.")]
    [HasDefaultValue]
    public string CsvDelimiter { get; set; } = ",";

    [Option("host", Description = "Smtp server host.")]
    [HasDefaultValue]
    public bool SmtpHost { get; set; }

    [Option("port", Description = "Smtp server port.")]
    [HasDefaultValue]
    public int SmtpPort { get; set; }

    [Option("username", Description = "Smtp server username.")]
    [HasDefaultValue]
    public bool SmtpUsername { get; set; }

    [Option("password", Description = "Smtp server password.")]
    [HasDefaultValue]
    public bool SmtpPassword { get; set; }
}
