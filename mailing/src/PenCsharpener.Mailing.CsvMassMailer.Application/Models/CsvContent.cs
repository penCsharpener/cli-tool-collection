namespace PenCsharpener.Mailing.CsvMassMailer.Application.Models;

public class CsvContent
{
    public IList<string>? EmailRecipients { get; internal set; }
    public IDictionary<string, IList<ReplacementPair>>? ReplacementTokens { get; set; }
}
