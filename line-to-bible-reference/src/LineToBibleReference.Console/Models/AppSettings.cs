namespace LineToBibleReference.Console.Models;
public class AppSettings
{
    public Dictionary<string, string> ConverterPathMapping { get; set; } = default!;
    public Dictionary<string, CsvPath> CsvPathMapping { get; set; } = default!;
}
