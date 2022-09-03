using System.Diagnostics.CodeAnalysis;

namespace PenCsharpener.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? enumerable)
    {
        return enumerable == null || !enumerable.Any();
    }
}
