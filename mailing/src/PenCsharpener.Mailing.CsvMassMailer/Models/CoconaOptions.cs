using Cocona;

namespace PenCsharpener.Mailing.CsvMassMailer.Models;

public class CoconaOptions : ICommandParameterSet
{
    [Option('m', Description = "Additionally to printing the email text also send them out directly using smtp settings.")]
    [HasDefaultValue]
    public bool SendEmails { get; set; } = false;

    [Option('d', Description = "Character used to separate columns in csv file.")]
    [HasDefaultValue]
    public string CsvDelimiter { get; set; } = "\",\"";

    [Option('h', Description = "Smtp server host.")]
    [HasDefaultValue]
    public string SmtpHost { get; set; } = default!;

    [Option('p', Description = "Smtp server port.")]
    [HasDefaultValue]
    public int SmtpPort { get; set; }

    [Option('u', Description = "Smtp server username.")]
    [HasDefaultValue]
    public string SmtpUsername { get; set; } = default!;

    [Option('n', Description = "Smtp sender full name.")]
    public string SmtpSenderName { get; set; } = default!;

    [Option('a', Description = "Smtp sender address.")]
    public string SmtpSenderAddress { get; set; } = default!;

    [Option('s', Description = "Smtp server password.")]
    [HasDefaultValue]
    public string SmtpPassword { get; set; } = default!;
}
