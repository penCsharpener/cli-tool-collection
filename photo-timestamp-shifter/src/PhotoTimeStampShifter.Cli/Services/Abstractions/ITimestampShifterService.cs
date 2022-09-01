namespace PhotoTimeStampShifter.Cli.Services.Abstractions;

public interface ITimestampShifterService
{
    IEnumerable<string> RenameTimeStamp();
}