using PhotoRename.Common.Models;
using PhotoRenamer.Cli.Services.Abstractions;

namespace PhotoRenamer.Cli.Services.FileNameStrategies;

public class CustomRegexFormattingStrategy : IFileNameStrategy
{
    private readonly FileName _fileName;
    private readonly string _customRegex;
    private readonly IImageSharpWrapper _imageSharpWrapper;

    public CustomRegexFormattingStrategy(FileName fileName, string customRegex, IImageSharpWrapper imageSharpWrapper)
    {
        _fileName = fileName;
        _customRegex = customRegex;
        _imageSharpWrapper = imageSharpWrapper;
    }

    public async Task<RenamePair?> GetRenamePair(CancellationToken token)
    {
        var creationDate = default(DateTime?);
        
        try
        {
            creationDate = await _imageSharpWrapper.GetCreationDate(_fileName.FullPath, token);
        }
        catch (Exception _)
        {
            // ignored
        }

        if (creationDate is null)
        {
            var fi = new FileInfo(_fileName.FullName);
            creationDate = fi.LastWriteTime;
        }

        return new(_fileName.FullPath, $"{creationDate:yyyyMMdd_HHmmss} {_fileName.Name}{_fileName.FileExtension}") { UsedExifTimestamp = true };
    }
}
