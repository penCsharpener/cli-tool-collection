using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Extensions;
public static class MappingExtensions
{
    public static WordMorphologyModel ToWordMorphologyModel(this WordMorphologyRawModel raw)
    {
        return new()
        {
            Resource = raw.Resource,
            Book = raw.Book,
            Chapter = raw.Chapter,
            Reference = raw.Reference,
            PreviousContext = raw.PreviousContext,
            Result = raw.Result,
            NextContext = raw.NextContext,
            Form = raw.Form,
            HebrewStrongs = raw.HebrewStrongs,
            LemmaHebrew = raw.LemmaHebrew,
            Root = raw.Root,
            Sense = raw.Sense,
            PartOfSpeech = raw.PartOfSpeech,
            ConjunctionTypeHebrew = raw.ConjunctionTypeHebrew,
            EndingTypeHebrew = raw.EndingTypeHebrew,
            GenderHebrew = raw.GenderHebrew,
            NounTypeHebrew = raw.NounTypeHebrew,
            NumberHebrew = raw.NumberHebrew,
            NumeralTypeHebrew = raw.NumeralTypeHebrew,
            PersonHebrew = raw.PersonHebrew,
            PrepositionTypeHebrew = raw.PrepositionTypeHebrew,
            PronounTypeHebrew = raw.PronounTypeHebrew,
            StateHebrew = raw.StateHebrew,
            StemHebrew = raw.StemHebrew,
            TAMHebrew = raw.TAMHebrew,
            YiqtōlVolitivesHebrew = raw.YiqtōlVolitivesHebrew,
            GreekStrongs = raw.GreekStrongs,
            LemmaGreek = raw.LemmaGreek,
            LouwNida = raw.LouwNida,
            AdverbParticleTypeGreek = raw.AdverbParticleTypeGreek,
            CaseGreek = raw.CaseGreek,
            ConjunctionSubtypeGreek = raw.ConjunctionSubtypeGreek,
            DegreeGreek = raw.DegreeGreek,
            GenderGreek = raw.GenderGreek,
            IndeclinableTypeGreek = raw.IndeclinableTypeGreek,
            MoodGreek = raw.MoodGreek,
            NumberGreek = raw.NumberGreek,
            PersonGreek = raw.PersonGreek,
            PronounSubtypeGreek = raw.PronounSubtypeGreek,
            PronounTypeGreek = raw.PronounTypeGreek,
            TenseGreek = raw.TenseGreek,
            VoiceGreek = raw.VoiceGreek,


        };
    }
}
