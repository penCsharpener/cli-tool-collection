using System.Text;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public sealed class WordStatistics : IWordStatistics
{
    private readonly IDiacriticRemovalServiceFactory _diacriticRemovalFactory;

    public WordStatistics(IDiacriticRemovalServiceFactory diacriticRemovalFactory)
    {
        _diacriticRemovalFactory = diacriticRemovalFactory;
    }

    public IEnumerable<WordStatsItem> GetBibleWordStats(string converterType, List<BibleVerseModel> bibleVerseModels)
    {
        var diacriticsRemovalService = _diacriticRemovalFactory.GetDiacriticRemovalService(converterType);
        var dict = new Dictionary<string, WordStatsItem>();

        foreach (var model in bibleVerseModels)
        {
            if (model.Words is null)
            {
                continue;
            }

            foreach (var word in diacriticsRemovalService.RemoveDiacritics(model.Words))
            {
                if (dict.ContainsKey(word))
                {
                    dict[word].Word = word;
                    dict[word].Count++;
                    dict[word].BibleReferences.Add(model.BibleReference);
                }
                else
                {
                    dict.Add(word, new() { Word = word, Count = 1 });
                }
            }
        }

        return dict.Values.OrderByDescending(m => m.Count);
    }

    public string PrintBibleBooks(IEnumerable<BibleVerseModel> bibleVerseModels)
    {
        var sb = new StringBuilder();

        foreach (var bookAbbreviation in bibleVerseModels.Select(m => m.BookAbbreviation).Distinct())
        {
            sb.AppendLine(bookAbbreviation);
        }

        return sb.ToString();
    }
}
