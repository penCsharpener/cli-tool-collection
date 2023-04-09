using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;
using LineToBibleReference.Console.Models.Enums;

namespace LineToBibleReference.Console.Services;

public sealed class GreekDiacriticRemovalService : IDiacriticRemovalService
{
    private static readonly CharacterReplacementModel[] _replacementList;

    static GreekDiacriticRemovalService()
    {
        _replacementList = Constants.GreekCharacterList
            .Where(c => c.CharacterTypes.HasValue && (
            c.CharacterTypes.Value.HasFlag(CharacterTypes.Punctuation) ||
            c.CharacterTypes.Value.HasFlag(CharacterTypes.Accent) ||
            c.CharacterTypes.Value.HasFlag(CharacterTypes.Quotation) ||
            c.CharacterTypes.Value.HasFlag(CharacterTypes.Parentheses))
            )
            .Select(c => new CharacterReplacementModel(c.Character, c.DefaultCharacterEquivalent))
        .ToArray();
    }

    public string RemoveDiacritics(string word)
    {
        foreach (var replacementMap in _replacementList)
        {
            if (replacementMap.Replacement is null)
            {
                word = word.Replace(replacementMap.Character.ToString(), string.Empty);

                continue;
            }

            word = word.Replace(replacementMap.Character, replacementMap.Replacement.Value);
        }

        return word;
    }

    public IEnumerable<string> RemoveDiacritics(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            yield return RemoveDiacritics(word);
        }
    }
}