using System.Text.RegularExpressions;
using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class MotorolaImageStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private static readonly Regex _regex = new("(IMG_)(\\d{8}_\\d{6,9})(.*)");

    public MotorolaImageStrategy(FileName fileName)
    {
        _fileName = fileName;
    }

    public Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var matches = _regex.Match(_fileName.Name);
        var values = matches.Groups.Values.ToArray();

        var timestamp = string.Empty;

        if (values[2].ValueSpan.Length == 15)
        {
            timestamp = values[2].ValueSpan.ToString();
        }
        else if (values[2].ValueSpan.Length == 18)
        {
            timestamp = values[2].ValueSpan[..^3].ToString();
        }

        var newFileName = string.Concat(timestamp, "_IMG", values[3]);

        return Task.FromResult<RenamePair?>(new(_fileName.FullPath, newFileName + _fileName.FileExtension));
    }
}
