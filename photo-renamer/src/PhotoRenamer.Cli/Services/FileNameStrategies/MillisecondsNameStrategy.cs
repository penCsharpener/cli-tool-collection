using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class MillisecondsNameStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private static readonly Regex _regex = new("(\\d{8}_\\d{9})(.*)");

    public MillisecondsNameStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair> GetRenamePair(CancellationToken token)
    {
        var matches = _regex.Match(_fileName.Name);
        var values = matches.Groups.Values.ToArray();

        var newFileName = string.Concat(values[1].ValueSpan[..^3].ToString(), values[2]);
        return Task.FromResult(new RenamePair(_fileName.FullName, $"{newFileName}{_fileName.FileExtension}"));
    }
}

