namespace LineToBibleReference.Console.Models.Enums;

[Flags]
public enum CharacterTypes
{
    Letter = 1,
    Accent = 1 << 1,
    Punctuation = 1 << 2,
    Vowel = 1 << 3,
    CapitalLetter = 1 << 4,
    Space = 1 << 5,
    Quotation = 1 << 6,
    Parentheses = 1 << 7,
    FinalOnly = 1 << 8,
}

[Flags]
public enum PartOfSpeech
{
    Adjective = 1,
    Adverb = 1 << 1,
    Article = 1 << 2,
    Conjunction = 1 << 3,
    Indeclinable = 1 << 4,
    Interjection = 1 << 5,
    Interrogative = 1 << 6,
    Negation = 1 << 7,
    Noun = 1 << 8,
    Numeral = 1 << 9,
    Particle = 1 << 10,
    Preposition = 1 << 11,
    Pronoun = 1 << 12,
    Verb = 1 << 13,
}

[Flags]
public enum AdverbParticleTypeGreek
{
    Place = 1,
    Negative = 2,
    Interrogative = 4,
    Correlative = 8,
    Emphatic = 16,
    Superlative = 32,
    Indefinite = 64,
    Conditional = 128
}

[Flags]
public enum CaseGreek
{
    Accusative = 1,
    Genitive = 2,
    Nominative = 4,
    Dative = 8,
    Vocative = 16
}

[Flags]
public enum ConjunctionSubtype
{
    AdverbialPurpose = 1,
    AdverbialComparative = 1 << 1,
    AdverbialConditional = 1 << 2,
    AdverbialTemporal = 1 << 3,
    LogicalDisjunctive = 1 << 4,
    LogicalConnective = 1 << 5,
    AdverbialLocal = 1 << 6,
    LogicalContrastive = 1 << 7,
    LogicalAscensive = 1 << 8,
    LogicalCorrelative = 1 << 9,
    AdverbialConcessive = 1 << 10,
    substantivalContent = 1 << 11,
    LogicalInferential = 1 << 12,
    AdverbialCausal = 1 << 13,
    LogicalExplanatory = 1 << 14,
    AdverbialResult = 1 << 15,
    substantivalEpexegetical = 1 << 16,
    LogicalTransitional = 1 << 17,
    LogicalEmphatic = 1 << 18,
    Relative = 1 << 19
}

public enum DegreeGreek
{
    Comparative,
    Superlative
}

[Flags]
public enum Gender
{
    Masculine = 1,
    Feminine = 1 << 1,
    Neuter = 1 << 2
}

public enum IndeclinableTypeGreek
{
    Foreign,
    ProperNoun,
    Numeral,
    Letter
}

[Flags]
public enum MoodGreek
{
    Participle = 1,
    Indicative = 1 << 1,
    Infinitive = 1 << 2,
    Imperative = 1 << 3,
    Optative = 1 << 4,
    Subjunctive = 1 << 5
}

[Flags]
public enum Number
{
    Singular = 1,
    Plural = 1 << 1,
    Dual = 1 << 2
}

[Flags]
public enum Person
{
    FirstPerson = 1,
    SecondPerson = 1 << 1,
    ThirdPerson = 1 << 2
}

public enum PronounSubtypeGreek
{
    IntensiveAttributive,
    IntensivePredicative
}

[Flags]
public enum PronounTypeGreek
{
    Relative = 1,
    Personal = 1 << 1,
    Indefinite = 1 << 2,
    Interrogative = 1 << 3,
    Demonstrative = 1 << 4,
    Reciprocal = 1 << 5,
    Reflexive = 1 << 6,
    Possessive = 1 << 7,
    Correlative = 1 << 8
}

[Flags]
public enum TenseGreek
{
    Present = 1,
    Aorist = 1 << 1,
    Perfect = 1 << 2,
    Future = 1 << 3,
    Imperfect = 1 << 4,
    Pluperfect = 1 << 5
}

[Flags]
public enum VoiceGreek
{
    Passive = 1,
    Active = 1 << 1,
    Middle = 1 << 2,
    MiddleOrPassive = 1 << 3
}

public enum EndingTypeHebrew
{
    LocativeHe,
    EnergicNun,
    ParagogicNun,
    ParagogicHe
}

public enum NounTypeHebrew
{
    Common = 1,
    Proper = 1 << 1
}

public enum PronounTypeHebrew
{
    Suffixed = 1,
    Demonstrative = 1 << 1,
    Personal = 1 << 2,
    Interrogative = 1 << 3
}

public enum StateHebrew
{
    Construct = 1,
    Absolute = 1 << 1
}

[Flags]
public enum StemHebrew : long
{
    Qal = 1,
    Piel = 1 << 1,
    Hifil = 1 << 2,
    Pual = 1 << 3,
    Pilel = 1 << 4,
    Hofal = 1 << 5,
    Nifal = 1 << 6,
    Hitpael = 1 << 7,
    Poel = 1 << 8,
    Hitpoel = 1 << 9,
    Hitpolel = 1 << 10,
    Hitpalpel = 1 << 11,
    Estafel = 1 << 12,
    Pilpel = 1 << 13,
    Polel = 1 << 14,
    Hitpolal = 1 << 15,
    Tifel = 1 << 16,
    Poal = 1 << 17,
    Pualal = 1 << 18,
    QalPassive = 1 << 19,
    Hotpaal = 1 << 20,
    Hotpael = 1 << 21,
    Nitpael = 1 << 22,
    Hitpael2 = 1 << 23,
    Pulal = 1 << 24,
    Polpal = 1 << 25,
    Polal = 1 << 26,
    Etpaal = 1 << 27,
    Palal = 1 << 28,
    Poalal = 1 << 29,
    Pealal = 1 << 30,
    Etpoel = 1 << 31,
    Pilal = 1 << 32,
}

[Flags]
public enum TamHebrew
{
    Infinitive = 1,
    WQatalWawPerfect = 1 << 1,
    YiqtolImperfect = 1 << 2,
    Participle = 1 << 3,
    QatalPerfect = 1 << 4,
    WyiqtolWawConjuctiveImperfect = 1 << 5,
    WayyiqtolWawConjuctiveImperfect = 1 << 6,
    PassiveParticiple = 1 << 7,
    Imperfect = 1 << 8,
    Imperative = 1 << 9,
}

public enum YiqtolVolitivesHebrew
{
    Cohortative,
    Jussive
}

public enum NumeralTypeHebrew
{
    Cardinal,
    Ordinal
}

public enum PrepositionTypeHebrew
{
    ObjectMarker
}