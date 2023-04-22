namespace LineToBibleReference.Console.Models;

public class CsvPath
{
    public string Path { get; set; } = default!;
    public string FileFilter { get; set; } = default!;
    public string RegexFilter { get; set; } = default!;
}