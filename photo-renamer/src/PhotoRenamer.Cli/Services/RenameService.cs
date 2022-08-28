using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using PhotoRenamer.Cli.Models;
using PhotoRenamer.Cli.Services.Abstractions;
using SixLabors.ImageSharp;

namespace PhotoRenamer.Cli.Services;

public class RenameService : IRenameService
{
    private readonly IHostEnvironment _hostEnvironment;

    public RenameService(IHostEnvironment hostEnvironment)
    {
        Console.WriteLine(hostEnvironment.ContentRootPath);

        _hostEnvironment = hostEnvironment;
    }

    public async IAsyncEnumerable<string> GetNameCommandsAsync([EnumeratorCancellation] CancellationToken stoppingToken)
    {
        var files = DirSearch(_hostEnvironment.ContentRootPath, null).Where(f => !string.IsNullOrWhiteSpace(f) && FilterFiles(f)).ToList();

        await foreach (var file in FilterRenameableFiles(files, stoppingToken))
        {
            yield return GlobalOptions.PreferCmd ? file.CmdRenameCommand : file.PowershellRenameCommand;
        }
    }

    public static async IAsyncEnumerable<RenamePair> FilterRenameableFiles(IEnumerable<string> files, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileExtensions = Path.GetExtension(file);

            if (fileName.StartsWith("IMG_") || fileName.StartsWith("HIC_"))
            {
                if (fileName.Length == 22)
                {
                    var newFileName = string.Concat(fileName.AsSpan(4, 15), "_IMG");
                    yield return new(file, newFileName + fileExtensions);
                }

                if (fileName.Length == 8)
                {
                    using var image = await Image.LoadAsync(file, stoppingToken);
                    var exif = image.Metadata.ExifProfile;
                    var creationDateTag = exif.GetValue(SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag.DateTimeOriginal);
                    var creationDateElements = creationDateTag.Value.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    var creationDateString = $"{creationDateElements[0]}-{creationDateElements[1]}-{creationDateElements[2]}:{creationDateElements[3]}:{creationDateElements[4]}";
                    var creationDate = DateTime.Parse(creationDateString);

                    yield return new(file, $"{creationDate:yyyyMMdd_HHmmss}_{fileName}{fileExtensions}");
                }

                if (fileName.Length > 22)
                {
                    var newFileName = string.Concat(fileName.AsSpan(4, 15), "_IMG", fileName.AsSpan()[22..]);
                    yield return new(file, newFileName + fileExtensions);
                }
            }

            if (fileName.StartsWith("MVI_"))
            {
                var fi = new FileInfo(file);
                var creationDate = fi.LastWriteTime;

                yield return new(file, $"{creationDate:yyyyMMdd_HHmmss}_{fileName}{fileExtensions}");
            }

            if (fileName.StartsWith("VID_"))
            {
                var newFileName = string.Concat(fileName.AsSpan(4, 15), "_VID");

                yield return new(file, newFileName + fileExtensions);
            }
        }
    }

    public static IEnumerable<string> DirSearch(string sDir, params string[]? excludeDirs)
    {
        foreach (var f in GetFilesInDir(sDir))
        {
            yield return f;
        }

        foreach (var d in GetDirs(sDir).Where(d => excludeDirs == null || ExcludeFolders(sDir, excludeDirs)))
        {
            foreach (var f in GetFilesInDir(d))
            {
                yield return f;
            }

            foreach (var f in DirSearch(d, excludeDirs))
            {
                yield return f;
            }
        }
    }

    private static IEnumerable<string> GetFilesInDir(string sDir)
    {
        try
        {
            return Directory.GetFiles(sDir);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    private static bool ExcludeFolders(string dir, string[] excludeDirs)
    {
        return excludeDirs.Any(d => dir.Contains(d));
    }

    private static IEnumerable<string> GetDirs(string sDir)
    {
        try
        {
            return Directory.GetDirectories(sDir);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public bool FilterFiles(string file)
    {
        return file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase);
    }

    public IAsyncEnumerable<string> GetNameCommandsAsync(RenameParameters options, CancellationToken stoppingToken = default)
    {
        throw new NotImplementedException();
    }
}
