using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class MotorolaVideoStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private static readonly Regex _regex = new("(VID_)(\\d{8}_\\d{9})(.*)");

    public MotorolaVideoStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var matches = _regex.Match(_fileName.Name);
        var values = matches.Groups.Values.ToArray();

        var newFileName = string.Concat(values[2].ValueSpan[..^3].ToString(), "_VID", values[3]);
        return Task.FromResult<RenamePair?>(new(_fileName.FullName, newFileName + _fileName.FileExtension));
    }
}
