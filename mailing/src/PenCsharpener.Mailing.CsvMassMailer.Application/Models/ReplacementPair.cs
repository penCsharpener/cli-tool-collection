namespace PenCsharpener.Mailing.CsvMassMailer.Application.Models;

public struct ReplacementPair
{
    public ReplacementPair(string token, string value)
    {
        Token = token;
        Value = value;
    }

    public string Token { get; set; }
    public string Value { get; set; }
}