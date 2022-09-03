namespace PenCsharpener.Mailing.CsvMassMailer.Application.Models;

public class CsvContent
{
    public IList<string> EmailRecipients { get; internal set; } = default!;
    public IDictionary<string, IList<ReplacementPair>> ReplacementPairs { get; set; } = default!;
}
