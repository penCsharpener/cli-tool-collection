using Microsoft.Extensions.Hosting;
using PhotoRename.Common.Models;
using PhotoRename.Common.Services.Abstractions;
using PhotoTimeStampShifter.Cli.Services.Abstractions;

namespace PhotoTimeStampShifter.Cli.Services;

public class TimestampShifterService : ITimestampShifterService
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IFileService _fileService;

    public TimestampShifterService(IHostEnvironment hostEnvironment, IFileService fileService)
    {
        Console.WriteLine(hostEnvironment.ContentRootPath);

        _hostEnvironment = hostEnvironment;
        _fileService = fileService;
    }

    public IEnumerable<string> RenameTimeStamp(string timeShiftValue)
    {
        var files = _fileService.GetFiles(_hostEnvironment.ContentRootPath, null).Where(f => !string.IsNullOrWhiteSpace(f) && FilterFiles(f)).ToList();

        var renamePairs = TimeShiftRenameableFiles(files, ParseTimeShiftOption(timeShiftValue));

        foreach (var file in renamePairs)
        {
            yield return GlobalOptions.PreferCmd ? file.CmdRenameCommand : file.PowershellRenameCommand;
        }
    }

    private static IEnumerable<RenamePair> TimeShiftRenameableFiles(IEnumerable<string> files, TimeSpan shiftBy)
    {
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileExtensions = Path.GetExtension(file);

            if (fileName.Length == 24 || fileName.Contains("MVI") || fileName.Contains("stitch"))
            {
                var timeStampString = fileName[0..15];

                var timeStamp = new DateTime(
                    int.Parse(timeStampString[0..4]),
                    int.Parse(timeStampString[4..6]),
                    int.Parse(timeStampString[6..8]),
                    int.Parse(timeStampString[9..11]),
                    int.Parse(timeStampString[11..13]),
                    int.Parse(timeStampString[13..15])
                    );

                var newFileName = timeStamp.Add(shiftBy).ToString("yyyyMMdd_HHmmss") + fileName[15..];

                yield return new(file, newFileName + fileExtensions);
            }
        }
    }

    private TimeSpan ParseTimeShiftOption(string timeShiftValue)
    {
        return timeShiftValue switch
        {
            [.. var seconds, 's'] => TimeSpan.FromSeconds(int.TryParse(seconds, out var value) ? value : 0),
            [.. var minutes, 'm'] => TimeSpan.FromMinutes(int.TryParse(minutes, out var value) ? value : 0),
            [.. var hours, 'h'] => TimeSpan.FromHours(int.TryParse(hours, out var value) ? value : 0),
            [.. var days, 'd'] => TimeSpan.FromDays(int.TryParse(days, out var value) ? value : 0),
            _ => throw new ArgumentOutOfRangeException(nameof(timeShiftValue), timeShiftValue, $"Cannot parse parameter")
        };
    }

    public static bool FilterFiles(string file)
    {
        return file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)
        || file.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase);
    }
}
