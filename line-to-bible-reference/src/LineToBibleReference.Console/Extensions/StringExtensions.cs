using System.Diagnostics.CodeAnalysis;

namespace LineToBibleReference.Console.Extensions;
public static class StringExtensions
{
    public static IEnumerable<string> SplitAndTrim(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Enumerable.Empty<string>();
        }

        return input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
    }

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}
