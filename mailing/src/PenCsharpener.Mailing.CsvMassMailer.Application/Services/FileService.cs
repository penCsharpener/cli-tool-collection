using System.IO.Abstractions;
using Microsoft.Extensions.Hosting;
using PenCsharpener.Mailing.CsvMassMailer.Application.Models;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services;

public class FileService : IFileService
{
    private readonly IFileSystem _fileSystem;
    private readonly IHostEnvironment _hostEnvironment;

    public FileService(IFileSystem fileSystem, IHostEnvironment hostEnvironment)
    {
        _fileSystem = fileSystem;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string[]> ReadAllLinesOfCsvAsync(CancellationToken cancellationToken = default)
    {
        var filePath = _fileSystem.Directory.GetFiles(_hostEnvironment.ContentRootPath, "*.csv").First();

        return await _fileSystem.File.ReadAllLinesAsync(filePath, cancellationToken);
    }

    public async Task<string> ReadTemplateAsync(CancellationToken cancellationToken = default)
    {
        var filePath = _fileSystem.Directory.GetFiles(_hostEnvironment.ContentRootPath, "*.txt").First();

        return await _fileSystem.File.ReadAllTextAsync(filePath, cancellationToken);
    }

    public async Task WriteEmailsAsync(EmailText[] emails, CancellationToken cancellationToken = default)
    {
        foreach (var email in emails)
        {
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, $"{email.Recipient}.txt");

            await _fileSystem.File.WriteAllTextAsync(filePath, email.EmailBody, cancellationToken);
        }
    }
}