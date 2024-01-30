namespace SemiAutoTranslator.Console.Extensions;

public static class StringExtensions
{
    public static string GetLast(this string source, int numberOfChars)
    {
        if (numberOfChars >= source.Length)
        {
            return source;
        }

        return source[^numberOfChars..];
    }
}