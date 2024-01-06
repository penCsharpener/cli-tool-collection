using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Extensions;
using LineToBibleReference.Console.Models;
using LineToBibleReference.Console.Models.Enums;

namespace LineToBibleReference.Console.Services;

public class CsvValueParser : ICsvValueParser
{
    public CsvValueParser(ILogger<CsvValueParser> logger)
    {
        _logger = logger;
    }

    public WordMorphologyModel Parse(WordMorphologyRawModel raw)
    {
        var model = raw.ToWordMorphologyModel();

        try
        {
            foreach (var item in raw.PartOfSpeech.SplitAndTrim())
            {
                model.PartOfSpeech += _enumMappingPartOfSpeech[item];
            }

            if (!raw.AdverbParticleTypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.AdverbParticleTypeGreek.SplitAndTrim())
                {
                    value += _enumMappingAdverbParticleTypeGreek[item];
                }

                model.AdverbParticleTypeGreek = (AdverbParticleTypeGreek)value;
            }

            if (!raw.Root.IsNullOrWhiteSpace())
            {
                model.Roots = raw.Root.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim()).ToArray();
            }

            if (!raw.PronounSubtypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PronounSubtypeGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.PronounSubtypeGreek = (PronounSubtypeGreek)value;
            }

            if (!raw.TAMHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.TAMHebrew.SplitAndTrim())
                {
                    value += _enumMappingTamHebrew[item];
                }

                model.TAMHebrew = (TamHebrew)value;
            }

            if (!raw.CaseGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.CaseGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.CaseGreek = (CaseGreek)value;
            }

            if (!raw.ConjunctionTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.ConjunctionTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.ConjunctionTypeHebrew = (ConjunctionSubtype)value;
            }

            if (!raw.ConjunctionSubtypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.ConjunctionSubtypeGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.ConjunctionSubtypeGreek = (ConjunctionSubtype)value;
            }

            if (!raw.EndingTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.EndingTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.EndingTypeHebrew = (EndingTypeHebrew)value;
            }

            if (!raw.GenderHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.GenderHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Gender = (Gender)value;
            }

            if (!raw.GenderGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.GenderGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Gender = (Gender)value;
            }

            if (!raw.NounTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.NounTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.NounTypeHebrew = (NounTypeHebrew)value;
            }

            if (!raw.NumberHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.NumberHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Number = (Number)value;
            }

            if (!raw.NumberGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.NumberGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Number = (Number)value;
            }

            if (!raw.NumeralTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.NumeralTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.NumeralTypeHebrew = (NumeralTypeHebrew)value;
            }

            if (!raw.PersonHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PersonHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Person = (Person)value;
            }

            if (!raw.PersonGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PersonGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.Person = (Person)value;
            }

            if (!raw.PrepositionTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PrepositionTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.PrepositionTypeHebrew = (PrepositionTypeHebrew)value;
            }

            if (!raw.PronounTypeHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PronounTypeHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.PronounTypeHebrew = (PronounTypeHebrew)value;
            }

            if (!raw.StateHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.StateHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.StateHebrew = (StateHebrew)value;
            }

            if (!raw.StemHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.StemHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.StemHebrew = (StemHebrew)value;
            }

            if (!raw.YiqtolVolitivesHebrew.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.YiqtolVolitivesHebrew.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.YiqtolVolitivesHebrew = (YiqtolVolitivesHebrew)value;
            }

            if (!raw.DegreeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.DegreeGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.DegreeGreek = (DegreeGreek)value;
            }

            if (!raw.IndeclinableTypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.IndeclinableTypeGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.IndeclinableTypeGreek = (IndeclinableTypeGreek)value;
            }

            if (!raw.MoodGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.MoodGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.MoodGreek = (MoodGreek)value;
            }

            if (!raw.PronounSubtypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PronounSubtypeGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.PronounSubtypeGreek = (PronounSubtypeGreek)value;
            }

            if (!raw.PronounTypeGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.PronounTypeGreek.SplitAndTrim())
                {
                    value += _enumMappingPronounTypeGreek[item];
                }

                model.PronounTypeGreek = (PronounTypeGreek)value;
            }

            if (!raw.TenseGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.TenseGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.TenseGreek = (TenseGreek)value;
            }

            if (!raw.VoiceGreek.IsNullOrWhiteSpace())
            {
                var value = 0;

                foreach (var item in raw.VoiceGreek.SplitAndTrim())
                {
                    value += _enumMapping[item];
                }

                model.VoiceGreek = (VoiceGreek)value;
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
        }

        return model;
    }

    private static readonly Dictionary<string, int> _enumMappingAdverbParticleTypeGreek = new()
    {
        { "—", 0 },
        { nameof(AdverbParticleTypeGreek.Place).ToLower(), (int) AdverbParticleTypeGreek.Place },
        { nameof(AdverbParticleTypeGreek.Negative).ToLower(), (int) AdverbParticleTypeGreek.Negative },
        { nameof(AdverbParticleTypeGreek.Interrogative).ToLower(), (int) AdverbParticleTypeGreek.Interrogative },
        { nameof(AdverbParticleTypeGreek.Correlative).ToLower(), (int) AdverbParticleTypeGreek.Correlative },
        { nameof(AdverbParticleTypeGreek.Emphatic).ToLower(), (int) AdverbParticleTypeGreek.Emphatic },
        { nameof(AdverbParticleTypeGreek.Superlative).ToLower(), (int) AdverbParticleTypeGreek.Superlative },
        { nameof(AdverbParticleTypeGreek.Indefinite).ToLower(), (int) AdverbParticleTypeGreek.Indefinite },
        { nameof(AdverbParticleTypeGreek.Conditional).ToLower(), (int) AdverbParticleTypeGreek.Conditional },
    };

    private static readonly Dictionary<string, int> _enumMappingPronounTypeGreek = new()
    {
        { "—", 0 },
        { nameof(PronounTypeGreek.Relative).ToLower(), (int) PronounTypeGreek.Relative },
        { nameof(PronounTypeGreek.Personal).ToLower(), (int) PronounTypeGreek.Personal },
        { nameof(PronounTypeGreek.Indefinite).ToLower(), (int) PronounTypeGreek.Indefinite },
        { nameof(PronounTypeGreek.Interrogative).ToLower(), (int) PronounTypeGreek.Interrogative },
        { nameof(PronounTypeGreek.Demonstrative).ToLower(), (int) PronounTypeGreek.Demonstrative },
        { nameof(PronounTypeGreek.Reciprocal).ToLower(), (int) PronounTypeGreek.Reciprocal },
        { nameof(PronounTypeGreek.Reflexive).ToLower(), (int) PronounTypeGreek.Reflexive },
        { nameof(PronounTypeGreek.Possessive).ToLower(), (int) PronounTypeGreek.Possessive },
        { nameof(PronounTypeGreek.Correlative).ToLower(), (int) PronounTypeGreek.Correlative },
    };

    private static readonly Dictionary<string, int> _enumMappingPartOfSpeech = new()
    {
        { "—", 0 },
        { nameof(PartOfSpeech.Article).ToLower(), (int) PartOfSpeech.Article },
        { nameof(PartOfSpeech.Adjective).ToLower(), (int) PartOfSpeech.Adjective },
        { nameof(PartOfSpeech.Adverb).ToLower(), (int) PartOfSpeech.Adverb },
        { nameof(PartOfSpeech.Conjunction).ToLower(), (int) PartOfSpeech.Conjunction },
        { nameof(PartOfSpeech.Indeclinable).ToLower(), (int) PartOfSpeech.Indeclinable },
        { nameof(PartOfSpeech.Interjection).ToLower(), (int) PartOfSpeech.Interjection },
        { nameof(PartOfSpeech.Interrogative).ToLower(), (int) PartOfSpeech.Interrogative },
        { nameof(PartOfSpeech.Negation).ToLower(), (int) PartOfSpeech.Negation },
        { nameof(PartOfSpeech.Noun).ToLower(), (int) PartOfSpeech.Noun },
        { nameof(PartOfSpeech.Numeral).ToLower(), (int) PartOfSpeech.Numeral },
        { nameof(PartOfSpeech.Particle).ToLower(), (int) PartOfSpeech.Particle },
        { nameof(PartOfSpeech.Preposition).ToLower(), (int) PartOfSpeech.Preposition },
        { nameof(PartOfSpeech.Pronoun).ToLower(), (int) PartOfSpeech.Pronoun },
        { nameof(PartOfSpeech.Verb).ToLower(), (int) PartOfSpeech.Verb },
    };

    private static readonly Dictionary<string, int> _enumMappingTamHebrew = new()
    {
        { "—", 0 },
        { "infinitive", (int) TamHebrew.Infinitive },
        { "wᵊqātal (waw + perfect)", (int) TamHebrew.WQatalWawPerfect },
        { "yiqtōl (imperfect)", (int) TamHebrew.YiqtolImperfect },
        { "participle", (int) TamHebrew.Participle },
        { "qātal (perfect)", (int) TamHebrew.QatalPerfect },
        { "wᵊyiqtōl (waw-conjunctive + imperfect)", (int) TamHebrew.WyiqtolWawConjuctiveImperfect },
        { "wayyiqtōl (waw-consecutive + imperfect)", (int) TamHebrew.WayyiqtolWawConjuctiveImperfect },
        { "passive participle", (int) TamHebrew.PassiveParticiple },
        { nameof(TamHebrew.Imperfect).ToLower(), (int) TamHebrew.Imperfect },
        { "imperative", (int) TamHebrew.Imperative },
    };

    private static readonly Dictionary<string, int> _enumMapping = new()
    {
        { "—", 0 },
        { nameof(CaseGreek.Accusative).ToLower(), (int) CaseGreek.Accusative },
        { nameof(CaseGreek.Genitive).ToLower(), (int) CaseGreek.Genitive },
        { nameof(CaseGreek.Nominative).ToLower(), (int) CaseGreek.Nominative },
        { nameof(CaseGreek.Dative).ToLower(), (int) CaseGreek.Dative },
        { nameof(CaseGreek.Vocative).ToLower(), (int) CaseGreek.Vocative },
        { "adverbial purpose", (int) ConjunctionSubtype.AdverbialPurpose },
        { "adverbial comparative", (int) ConjunctionSubtype.AdverbialComparative },
        { "adverbial conditional", (int) ConjunctionSubtype.AdverbialConditional },
        { "adverbial temporal", (int) ConjunctionSubtype.AdverbialTemporal },
        { "logical disjunctive", (int) ConjunctionSubtype.LogicalDisjunctive },
        { "logical connective", (int) ConjunctionSubtype.LogicalConnective },
        { "adverbial local", (int) ConjunctionSubtype.AdverbialLocal },
        { "logical contrastive", (int) ConjunctionSubtype.LogicalContrastive },
        { "logical ascensive", (int) ConjunctionSubtype.LogicalAscensive },
        { "logical correlative", (int) ConjunctionSubtype.LogicalCorrelative },
        { "adverbial concessive", (int) ConjunctionSubtype.AdverbialConcessive },
        { "substantival content", (int) ConjunctionSubtype.substantivalContent },
        { "logical inferential", (int) ConjunctionSubtype.LogicalInferential },
        { "adverbial causal", (int) ConjunctionSubtype.AdverbialCausal },
        { "logical explanatory", (int) ConjunctionSubtype.LogicalExplanatory },
        { "adverbial result", (int) ConjunctionSubtype.AdverbialResult },
        { "substantival epexegetical", (int) ConjunctionSubtype.substantivalEpexegetical },
        { "logical transitional", (int) ConjunctionSubtype.LogicalTransitional },
        { "logical emphatic", (int) ConjunctionSubtype.LogicalEmphatic },
        { "relative", (int) ConjunctionSubtype.Relative },
        { nameof(DegreeGreek.Comparative).ToLower(), (int) DegreeGreek.Comparative },
        { nameof(DegreeGreek.Superlative).ToLower(), (int) DegreeGreek.Superlative },
        { nameof(Gender.Masculine).ToLower(), (int) Gender.Masculine },
        { nameof(Gender.Feminine).ToLower(), (int) Gender.Feminine },
        { nameof(Gender.Neuter).ToLower(), (int) Gender.Neuter },
        { nameof(IndeclinableTypeGreek.Foreign).ToLower(), (int) IndeclinableTypeGreek.Foreign },
        { "proper noun", (int) IndeclinableTypeGreek.ProperNoun },
        { nameof(IndeclinableTypeGreek.Numeral).ToLower(), (int) IndeclinableTypeGreek.Numeral },
        { nameof(IndeclinableTypeGreek.Letter).ToLower(), (int) IndeclinableTypeGreek.Letter },
        { nameof(MoodGreek.Participle).ToLower(), (int) MoodGreek.Participle },
        { nameof(MoodGreek.Indicative).ToLower(), (int) MoodGreek.Indicative },
        { nameof(MoodGreek.Infinitive).ToLower(), (int) MoodGreek.Infinitive },
        { nameof(MoodGreek.Imperative).ToLower(), (int) MoodGreek.Imperative },
        { nameof(MoodGreek.Optative).ToLower(), (int) MoodGreek.Optative },
        { nameof(MoodGreek.Subjunctive).ToLower(), (int) MoodGreek.Subjunctive },
        { nameof(Number.Singular).ToLower(), (int) Number.Singular },
        { nameof(Number.Plural).ToLower(), (int) Number.Plural },
        { nameof(Number.Dual).ToLower(), (int) Number.Dual },
        { "first person", (int) Person.FirstPerson },
        { "second person", (int) Person.SecondPerson },
        { "third person", (int) Person.ThirdPerson },
        { "intensive attributive", (int) PronounSubtypeGreek.IntensiveAttributive },
        { "intensive predicative", (int) PronounSubtypeGreek.IntensivePredicative },
        { nameof(TenseGreek.Present).ToLower(), (int) TenseGreek.Present },
        { nameof(TenseGreek.Aorist).ToLower(), (int) TenseGreek.Aorist },
        { nameof(TenseGreek.Perfect).ToLower(), (int) TenseGreek.Perfect },
        { nameof(TenseGreek.Future).ToLower(), (int) TenseGreek.Future },
        { nameof(TenseGreek.Imperfect).ToLower(), (int) TenseGreek.Imperfect },
        { nameof(TenseGreek.Pluperfect).ToLower(), (int) TenseGreek.Pluperfect },
        { nameof(VoiceGreek.Passive).ToLower(), (int) VoiceGreek.Passive },
        { nameof(VoiceGreek.Active).ToLower(), (int) VoiceGreek.Active },
        { nameof(VoiceGreek.Middle).ToLower(), (int) VoiceGreek.Middle },
        { "either middle or passive", (int) VoiceGreek.MiddleOrPassive },
        { "locative hē", (int) EndingTypeHebrew.LocativeHe },
        { "energic nûn", (int) EndingTypeHebrew.EnergicNun },
        { "paragogic nûn", (int) EndingTypeHebrew.ParagogicNun },
        { "paragogic hē", (int) EndingTypeHebrew.ParagogicHe },
        { nameof(NounTypeHebrew.Common).ToLower(), (int) NounTypeHebrew.Common },
        { nameof(NounTypeHebrew.Proper).ToLower(), (int) NounTypeHebrew.Proper },
        { nameof(PronounTypeHebrew.Suffixed).ToLower(), (int) PronounTypeHebrew.Suffixed },
        { nameof(PronounTypeHebrew.Demonstrative).ToLower(), (int) PronounTypeHebrew.Demonstrative },
        { nameof(PronounTypeHebrew.Personal).ToLower(), (int) PronounTypeHebrew.Personal },
        { nameof(PronounTypeHebrew.Interrogative).ToLower(), (int) PronounTypeHebrew.Interrogative },
        { nameof(StateHebrew.Construct).ToLower(), (int) StateHebrew.Construct },
        { nameof(StateHebrew.Absolute).ToLower(), (int) StateHebrew.Absolute },
        { nameof(StemHebrew.Qal), (int) StemHebrew.Qal },
        { "Piʿʿēl", (int) StemHebrew.Piel },
        { "Hifʿîl", (int) StemHebrew.Hifil },
        { "Puʿʿal", (int) StemHebrew.Pual },
        { "Piʿlēl", (int) StemHebrew.Pilel },
        { "Hofʿal", (int) StemHebrew.Hofal },
        { "Nifʿal", (int) StemHebrew.Nifal },
        { "Hiṯpaʿʿēl", (int) StemHebrew.Hitpael },
        { "Pôʿēl", (int) StemHebrew.Poel },
        { "Hiṯpôʿēl", (int) StemHebrew.Hitpoel },
        { "Hiṯpôlēl", (int) StemHebrew.Hitpolel },
        { "Hiṯpalpēl", (int) StemHebrew.Hitpalpel },
        { "Eštafʿēl", (int) StemHebrew.Estafel },
        { "Pilpēl", (int) StemHebrew.Pilpel },
        { "Pôlēl", (int) StemHebrew.Polel },
        { "Hiṯpôlal", (int) StemHebrew.Hitpolal },
        { "Tifʿēl", (int) StemHebrew.Tifel },
        { "Pôʿal", (int) StemHebrew.Poal },
        { "Puʿalal", (int) StemHebrew.Pualal },
        { "Qal Passive", (int) StemHebrew.QalPassive },
        { "Hoṯpaʿʿal", (int) StemHebrew.Hotpaal },
        { "Hoṯpāʿēl", (int) StemHebrew.Hotpael },
        { "Niṯpaʿʿēl", (int) StemHebrew.Nitpael },
        { "Hiṯpāʿēl", (int) StemHebrew.Hitpael2 },
        { "Puʿlal", (int) StemHebrew.Pulal },
        { "Polpal", (int) StemHebrew.Polpal },
        { "Pôlal", (int) StemHebrew.Polal },
        { "ʾEṯpaʿʿal", (int) StemHebrew.Etpaal },
        { "Paʿlal", (int) StemHebrew.Palal },
        { "Pᵒʿalʿal", (int) StemHebrew.Poalal },
        { "Pᵊʿalʿal", (int) StemHebrew.Pealal },
        { "ʾEṯpôʿēl", (int) StemHebrew.Etpoel },
        { "Piʿlal", (int) StemHebrew.Pilal },

        { nameof(YiqtolVolitivesHebrew.Cohortative).ToLower(), (int) YiqtolVolitivesHebrew.Cohortative },
        { nameof(YiqtolVolitivesHebrew.Jussive).ToLower(), (int) YiqtolVolitivesHebrew.Jussive },
        { nameof(NumeralTypeHebrew.Cardinal).ToLower(), (int) NumeralTypeHebrew.Cardinal },
        { nameof(NumeralTypeHebrew.Ordinal).ToLower(), (int) NumeralTypeHebrew.Ordinal },
        { "object marker", (int) PrepositionTypeHebrew.ObjectMarker },
    };
    private readonly ILogger<CsvValueParser> _logger;
}
