using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface IRootGroupingService
{
    Dictionary<string, List<WordMorphologyModel>> GroupByRoot(List<WordMorphologyModel> models);
}