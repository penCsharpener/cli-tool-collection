using LineToBibleReference.Console.Models.Enums;

namespace LineToBibleReference.Console.Models;

public class CharacterStatsItem
{
    public char Character { get; }
    public int CharacterValue { get; }
    public int Count { get; }
    public CharacterTypes? CharacterTypes { get; }
    public string Utf8Code => $"\\u{(int)Character:x4}";
    public string? Description { get; set; }

    public CharacterStatsItem(char character, int characterValue, int count)
    {
        Character = character;
        CharacterValue = characterValue;
        Count = count;
    }


    public CharacterStatsItem(char character, int characterValue, int count, CharacterTypes characterTypes) : this(character, characterValue, count)
    {
        CharacterTypes = characterTypes;
    }

    public override string ToString()
    {
        return $"{Character}: {CharacterValue,5}, {Utf8Code}";
    }
}