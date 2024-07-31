using PhotoRename.Common.Models;

namespace PhotoRenamer.Cli.Services.Abstractions;

public interface IFileNameStrategyFactory
{
    IFileNameStrategy GetStrategy(FileName fileName);
}
