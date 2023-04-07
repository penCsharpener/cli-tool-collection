using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Abstractions;

public interface IWordStatistics
{
    IEnumerable<WordStatsItem> GetBibleWordStats(List<BibleVerseModel> bibleVerseModels);
    string PrintBibleBooks(IEnumerable<BibleVerseModel> bibleVerseModels);
}