namespace LineToBibleReference.Console.Models.Enums;

[Flags]
public enum CharacterTypes
{
    Letter = 1,
    Accent = 2,
    Punctuation = 4,
    Vowel = 8,
    CapitalLetter = 16,
    Space = 32,
    Quotation = 64,
    Parentheses = 128,
}
