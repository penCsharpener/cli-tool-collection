using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface IValueGrouper
{
    Task WriteAllUnitValues(IEnumerable<WordMorphologyRawModel> values);
}