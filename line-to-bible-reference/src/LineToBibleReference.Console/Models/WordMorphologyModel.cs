using LineToBibleReference.Console.Models.Enums;

namespace LineToBibleReference.Console.Models;

public sealed class WordMorphologyRawModel
{
    public string? Resource { get; set; }
    public string? Book { get; set; }
    public string? Chapter { get; set; }
    public string? Reference { get; set; }
    public string? PreviousContext { get; set; }
    public string? Result { get; set; }
    public string? NextContext { get; set; }
    public string? Form { get; set; }
    public string? HebrewStrongs { get; set; }
    public string? LemmaHebrew { get; set; }
    public string? Root { get; set; }
    public string? Sense { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? ConjunctionTypeHebrew { get; set; }
    public string? EndingTypeHebrew { get; set; }
    public string? GenderHebrew { get; set; }
    public string? NounTypeHebrew { get; set; }
    public string? NumberHebrew { get; set; }
    public string? NumeralTypeHebrew { get; set; }
    public string? PersonHebrew { get; set; }
    public string? PrepositionTypeHebrew { get; set; }
    public string? PronounTypeHebrew { get; set; }
    public string? StateHebrew { get; set; }
    public string? StemHebrew { get; set; }
    public string? TAMHebrew { get; set; }
    public string? YiqtolVolitivesHebrew { get; set; }
    public string? GreekStrongs { get; set; }
    public string? LemmaGreek { get; set; }
    public string? LouwNida { get; set; }
    public string? AdverbParticleTypeGreek { get; set; }
    public string? CaseGreek { get; set; }
    public string? ConjunctionSubtypeGreek { get; set; }
    public string? DegreeGreek { get; set; }
    public string? GenderGreek { get; set; }
    public string? IndeclinableTypeGreek { get; set; }
    public string? MoodGreek { get; set; }
    public string? NumberGreek { get; set; }
    public string? PersonGreek { get; set; }
    public string? PronounSubtypeGreek { get; set; }
    public string? PronounTypeGreek { get; set; }
    public string? TenseGreek { get; set; }
    public string? VoiceGreek { get; set; }

}

public sealed class WordMorphologyModel
{
    public string? Resource { get; set; }
    public string? Book { get; set; }
    public string? Chapter { get; set; }
    public string? Reference { get; set; }
    public string? PreviousContext { get; set; }
    public string? Result { get; set; }
    public string? NextContext { get; set; }
    public string? Form { get; set; }
    public string? Root { get; set; }
    public string? Sense { get; set; }
    public PartOfSpeech PartOfSpeech { get; set; }
    public ConjunctionSubtype? ConjunctionTypeHebrew { get; set; }
    public EndingTypeHebrew? EndingTypeHebrew { get; set; }
    public Gender? Gender { get; set; }
    public NounTypeHebrew? NounTypeHebrew { get; set; }
    public Number? Number { get; set; }
    public NumeralTypeHebrew? NumeralTypeHebrew { get; set; }
    public Person? Person { get; set; }
    public PrepositionTypeHebrew? PrepositionTypeHebrew { get; set; }
    public PronounTypeHebrew? PronounTypeHebrew { get; set; }
    public StateHebrew? StateHebrew { get; set; }
    public StemHebrew? StemHebrew { get; set; }
    public TamHebrew? TAMHebrew { get; set; }
    public YiqtolVolitivesHebrew? YiqtolVolitivesHebrew { get; set; }
    public string? Strongs { get; set; }
    public string? Lemma { get; set; }
    public string? LouwNida { get; set; }
    public AdverbParticleTypeGreek? AdverbParticleTypeGreek { get; set; }
    public CaseGreek? CaseGreek { get; set; }
    public ConjunctionSubtype? ConjunctionSubtypeGreek { get; set; }
    public DegreeGreek? DegreeGreek { get; set; }
    public IndeclinableTypeGreek? IndeclinableTypeGreek { get; set; }
    public MoodGreek? MoodGreek { get; set; }
    public PronounSubtypeGreek? PronounSubtypeGreek { get; set; }
    public PronounTypeGreek? PronounTypeGreek { get; set; }
    public TenseGreek? TenseGreek { get; set; }
    public VoiceGreek? VoiceGreek { get; set; }

}