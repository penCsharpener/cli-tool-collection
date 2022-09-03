using PenCsharpener.Mailing.CsvMassMailer.Models;

namespace PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;

public interface ICsvMassMailerService
{
    Task ExecuteAsync(CoconaOptions options, CancellationToken cancellationToken = default);
}
