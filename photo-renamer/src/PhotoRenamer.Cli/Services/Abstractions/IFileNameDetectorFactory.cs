using PhotoRename.Common.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IFileNameDetectorFactory
{
    IFileNameDetector GetDetector(FileName fileName);
}
