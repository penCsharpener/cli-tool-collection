using System.Text;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public class CharacterStatistics : ICharacterStatistics
{
    public Dictionary<char, int> GetLanguageStatistics(IEnumerable<BibleReferenceModel> bibleReferences)
    {
        var dict = new Dictionary<char, int>();

        foreach (var item in bibleReferences)
        {
            foreach (var c in item.Text.ToCharArray())
            {
                if (dict.TryGetValue(c, out var value))
                {
                    value++;
                    dict[c] = value;
                }
                else
                {
                    dict[c] = 1;
                }
            }
        }

        return dict;
    }

    public string PrintStats(Dictionary<char, int> dictionary)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"\t\tvar characterList = new List<CharacterStatsItem>()");
        sb.AppendLine("\t\t{");

        foreach (var item in dictionary)
        {
            sb.AppendLine($"\t\t\tnew('{item.Key}', {(int)item.Key}, {item.Value}),");
        }

        sb.AppendLine("\t\t};");

        return sb.ToString();
    }
}
