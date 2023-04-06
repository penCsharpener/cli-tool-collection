using LineToBibleReference.Console.Models.Enums;

namespace LineToBibleReference.Console.Models;

public class GreekCharacterStatsItem : CharacterStatsItem
{
    private readonly char? _defaultCharacterEquivalent;
    public char DefaultCharacterEquivalent => _defaultCharacterEquivalent ?? Character;

    public GreekCharacterStatsItem(char character, int characterValue, int count) : base(character, characterValue, count) { }

    public GreekCharacterStatsItem(char character, int characterValue, int count, CharacterTypes characterTypes)
        : base(character, characterValue, count, characterTypes) { }

    public GreekCharacterStatsItem(char character, int characterValue, int count, char? defaultCharacter, CharacterTypes characterTypes)
        : base(character, characterValue, count, characterTypes)
    {
        _defaultCharacterEquivalent = defaultCharacter;
    }
}