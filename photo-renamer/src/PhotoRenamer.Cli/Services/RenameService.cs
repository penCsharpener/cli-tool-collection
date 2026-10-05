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
    private readonly IFileNameStrategyFactory _fileNameDetectorFactory;
    private readonly IFileService _fileService;

    public RenameService(IHostEnvironment hostEnvironment, IFileNameStrategyFactory fileNameDetectorFactory, IFileService fileService)
    {
        Console.WriteLine(hostEnvironment.ContentRootPath);

        _hostEnvironment = hostEnvironment;
        _fileNameDetectorFactory = fileNameDetectorFactory;
        _fileService = fileService;
    }

    public async IAsyncEnumerable<RenamePair> GetNameCommandsAsync(RenameParameters options, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        var files = _fileService.GetFiles(_hostEnvironment.ContentRootPath, null).Where(f => !string.IsNullOrWhiteSpace(f) && (FilterImageFiles(f) || FilterVideoFiles(f)));

        await foreach (var file in FilterRenameableFiles(files, options, stoppingToken))
        {
            file.ApplyOptions(options.OnlyUseFilename);

            if (options.VerboseLogging)
            {
                Console.WriteLine($"\t\t\t\t\tfile: {file.NewFileName}");
            }

            if (options.ExcludeVideos && file.IsVideoFile)
            {
                continue;
            }

            if (options.ExifOnly && !file.UsedExifTimestamp && !file.IsAlreadyNamed)
            {
                continue;
            }

            yield return file;
        }
    }

    public async IAsyncEnumerable<RenamePair> FilterRenameableFiles(IEnumerable<string> files, RenameParameters options, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        // full paths that are already taken by an earlier rename in this run
        var claimedTargets = new HashSet<string>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        foreach (var file in files)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            var fileName = new FileName(file);
            var strategy = _fileNameDetectorFactory.GetStrategy(fileName, options.CustomRegex);

            if (options.VerboseLogging)
            {
                Console.WriteLine($"\t\t\t\t\tfound file: {file}");
                Console.WriteLine($"\t\t\t\t\tfilepath: {fileName.FullPath}");
                Console.WriteLine($"\t\t\t\t\tfilename: {fileName.FullName}");
                Console.WriteLine($"\t\t\t\t\tstrategy: {strategy.GetType().Name}");
            }

            RenamePair? result = null;

            try
            {
                result = await strategy.GetRenamePair(stoppingToken);
            }
            catch (Exception ex)
            {
                if (!options.NoErrorLogging)
                {
                    Console.WriteLine($"\t\t\t\t\tstrategy exception: {ex}");
                }
            }

            if (options.VerboseLogging)
            {
                Console.WriteLine($"\t\t\t\t\trename file: {result.FileInfo.FullName}");
                Console.WriteLine($"\t\t\t\t\tnew rename file: {result.NewFileInfo.FullName}");
            }

            if (result is null)
            {
                continue;
            }

            if (result.FileInfo.Name.Equals(result.NewFileName, StringComparison.OrdinalIgnoreCase))
            {
                // nothing to rename, but jpgs still have to be handed on for the webp conversion
                if (!options.ConvertToWebp || !IsJpeg(result.FileInfo.Name))
                {
                    continue;
                }

                result.IsAlreadyNamed = true;
            }

            // never produce a rename that would overwrite another file (e.g. photos that only differ in the removed milliseconds)
            if (!result.IsAlreadyNamed && (!claimedTargets.Add(result.NewFileInfo.FullName) || File.Exists(result.NewFileInfo.FullName)))
            {
                if (!options.NoErrorLogging)
                {
                    Console.WriteLine($"\t\t\t\t\tskipped, target already exists: {result.FileInfo.Name} ==> {result.NewFileInfo.Name}");
                }

                continue;
            }

            if (result.IsAlreadyNamed)
            {
                claimedTargets.Add(result.NewFileInfo.FullName);
            }

            if (FilterVideoFiles(result.FileInfo.Name))
            {
                result.IsVideoFile = true;
            }

            yield return result;

            continue;
        }
    }

    private static bool IsJpeg(string file)
    {
        return file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
            || file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase);
    }

    public bool FilterImageFiles(string file)
    {
        return file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase);
    }

    public bool FilterVideoFiles(string file)
    {
        return file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".mov", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".avi", StringComparison.OrdinalIgnoreCase);
    }
}
