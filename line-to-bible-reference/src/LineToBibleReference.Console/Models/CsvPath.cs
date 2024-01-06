namespace LineToBibleReference.Console.Models;

public class CsvPath
{
    public string Path { get; set; } = default!;
    public string FileFilter { get; set; } = default!;
    public string RegexFilter { get; set; } = default!;
    public string TargetPath { get; set; } = default!;
    public string TargetFileName { get; set; } = default!;
    public string TargetPathRootGrouping { get; set; } = default!;
    public string TargetFileNameRootGrouping { get; set; } = default!;
}