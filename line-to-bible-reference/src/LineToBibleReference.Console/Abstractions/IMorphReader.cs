using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface IMorphReader
{
    IAsyncEnumerable<WordMorphologyModel> ReadMorphologyAsync();
}