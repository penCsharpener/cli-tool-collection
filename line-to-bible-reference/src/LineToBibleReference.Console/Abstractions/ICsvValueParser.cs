using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface ICsvValueParser
{
    WordMorphologyModel Parse(WordMorphologyRawModel raw);
}