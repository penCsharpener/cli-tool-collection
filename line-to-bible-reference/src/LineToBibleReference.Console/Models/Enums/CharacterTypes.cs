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
    FinalOnly = 256,
}

[Flags]
public enum PartOfSpeech
{
    Adjective,
    Adverb,
    Article,
    Conjunction,
    Indeclinable,
    Interjection,
    Interrogative,
    Negation,
    Noun,
    Numeral,
    Particle,
    Preposition,
    Pronoun,
    Verb
}

[Flags]
public enum AdverbParticleTypeGreek
{
    Place,
    Negative,
    Interrogative,
    Correlative,
    Emphatic,
    Superlative,
    Indefinite,
    Conditional
}

[Flags]
public enum CaseGreek
{
    Accusative,
    Genetive,
    Nominative,
    Dative,
    Vocative
}

public enum ConjunctionSubtype
{
    AdverbialPurpose,
    AdverbialComparative,
    AdverbialConditional,
    LogicalDisjunctive,
    LogicalConnective,
    AdverbialLocal,
    LogicalContrastive,
    LogicalAscensive,
    LogicalCorrelative,
    AdverbialConcessive,
    substantivalContent,
    LogicalInferential,
    AdverbialCausal,
    LogicalExplanatory,
    AdverbialResult,
    substantivalEpexegetical,
    LogicalTransitional,
    LogicalEmphatic,
    Relative
}

public enum DegreeGreek
{
    Comparative,
    Superlative
}

[Flags]
public enum Gender
{
    Masculine,
    Feminine,
    Neuter
}

public enum IndeclinableTypeGreek
{
    Foreign,
    ProperNoun,
    Numeral,
    Letter
}

public enum MoodGreek
{
    Participle,
    Indicative,
    Infinitive,
    Imperative,
    Optative,
    Subjunctive
}

[Flags]
public enum Number
{
    Singular,
    Plural,
    Dual
}

public enum Person
{
    FirstPerson,
    SecondPerson,
    ThirdPerson
}

public enum PronounSubtypeGreek
{
    IntensiveAttributive,
    IntensivePredicative
}

public enum PronounTypeGreek
{
    Relative,
    Personal,
    Indefinite,
    Interrogative,
    Demonstrative,
    Reciprocal,
    Reflexive,
    Possessive,
    Correlative
}

public enum TenseGreek
{
    Present,
    Aorist,
    Perfect,
    Future,
    Imperfect,
    Pluperfect
}

[Flags]
public enum VoiceGreek
{
    Passive,
    Active,
    Middle,
    MiddleOrPassive
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
    Common,
    Proper
}

public enum PronounTypeHebrew
{
    Suffixed,
    Demonstrative,
    Personal,
    Interrogative
}

public enum StateHebrew
{
    Construct,
    Absolute
}

public enum StemHebrew
{
    Qal,
    Piel,
    Hifil,
    Pual,
    Pilel,
    Hofal,
    Nifal,
    Hitpael,
    Poel,
    Hitpoel,
    Hitpolel,
    Hitpalpel,
    Estafel,
    Pilpel,
    Polel,
    Hitpolal,
    Tifel,
    Poal,
    Pualal,
    QalPassive,
    Hotpaal,
    Hotpael,
    Nitpael,
    Hitpael2,
    Pulal,
    Polpal,
    Polal,
    Etpaal,
    Palal,
    Poalal,
    Pealal,
    Etpoel,
    Pilal
}

public enum TamHebrew
{
    Infinitive,
    WQatalWawPerfect,
    YiqtolImperfect,
    Participle,
    QatalPerfect,
    WyiqtolWawConjuctiveImperfect,
    WayyiqtolWawConjuctiveImperfect,
    PassiveParticiple,
    Imperfect
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