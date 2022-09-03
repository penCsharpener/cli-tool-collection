using PenCsharpener.Mailing.CsvMassMailer.Application.Models;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

public interface ICsvReader
{
    CsvContent ParseCsvContent(string[] csvLines);
}
