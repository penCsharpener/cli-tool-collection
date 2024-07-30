using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using PhotoRename.Common.Models;
using PhotoRename.Common.Services.Abstractions;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services;

public class RenameService : IRenameService
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IFileNameDetectorFactory _fileNameDetectorFactory;
    private readonly IFileService _fileService;

    public RenameService(IHostEnvironment hostEnvironment, IFileNameDetectorFactory fileNameDetectorFactory, IFileService fileService)
    {
        Console.WriteLine(hostEnvironment.ContentRootPath);

        _hostEnvironment = hostEnvironment;
        _fileNameDetectorFactory = fileNameDetectorFactory;
        _fileService = fileService;
    }

    public async IAsyncEnumerable<string> GetNameCommandsAsync(RenameParameters options, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        var files = _fileService.GetFiles(_hostEnvironment.ContentRootPath, null).Where(f => !string.IsNullOrWhiteSpace(f) && FilterFiles(f)).ToList();

        await foreach (var file in FilterRenameableFiles(files, stoppingToken))
        {
            file.ApplyOptions(options.OnlyUseFilename);

            yield return options.PreferCmd ? file.CmdRenameCommand : file.PowershellRenameCommand;
        }
    }

    public async IAsyncEnumerable<RenamePair> FilterRenameableFiles(IEnumerable<string> files, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        foreach (var file in files)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            var fileName = new FileName(file);
            var result = await _fileNameDetectorFactory.GetDetector(fileName).GetRenamePair(stoppingToken);

            if (result is null)
            {
                continue;
            }

            yield return result;

            continue;
        }
    }

    public bool FilterFiles(string file)
    {
        return file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase);
    }
}
