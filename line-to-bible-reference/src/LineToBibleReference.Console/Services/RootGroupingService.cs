using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public sealed class RootGroupingService : IRootGroupingService
{
    public Dictionary<string, List<WordMorphologyModel>> GroupByRoot(List<WordMorphologyModel> models)
    {
        var dict = new Dictionary<string, List<WordMorphologyModel>>();

        foreach (var model in models.Where(m => m.Roots is not null && m.Roots.Length > 0))
        {
            foreach (var root in model.Roots)
            {
                if (dict.ContainsKey(root))
                {
                    dict[root].Add(model);

                    continue;
                }

                dict.Add(root, new List<WordMorphologyModel>() { model });
            }
        }

        foreach (var kvp in dict)
        {
            dict[kvp.Key] = kvp.Value.Distinct().OrderBy(m => m.Reference).ToList();
        }

        dict = dict.OrderByDescending(m => m.Value.Count).ToDictionary(k => k.Key, v => v.Value);

        return dict;
    }
}
