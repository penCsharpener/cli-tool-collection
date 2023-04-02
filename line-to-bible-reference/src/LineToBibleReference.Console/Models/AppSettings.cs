namespace LineToBibleReference.Console.Models;
public class AppSettings
{
    public string PathToGermanTextFile { get; set; } = default!;
    public string PathToHebrewTextFile { get; set; } = default!;
    public string PathToGreekTextFile { get; set; } = default!;
    public string SelectedRegexPattern { get; set; } = default!;
}
