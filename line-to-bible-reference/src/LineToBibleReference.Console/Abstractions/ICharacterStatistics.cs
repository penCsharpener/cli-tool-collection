using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface ICharacterStatistics
{
    Dictionary<char, int> GetLanguageStatistics(IEnumerable<BibleVerseModel> bibleReferences);
    string PrintStats(Dictionary<char, int> dictionary);
}
