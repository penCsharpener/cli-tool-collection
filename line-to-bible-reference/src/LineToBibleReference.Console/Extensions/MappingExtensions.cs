﻿using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Extensions;
public static class MappingExtensions
{
    public static WordMorphologyModel ToWordMorphologyModel(this WordMorphologyRawModel raw)
    {
        return new()
        {
            Resource = raw.Resource,
            Reference = BibleReference.Parse(raw.Reference!),
            PreviousContext = raw.PreviousContext,
            Result = raw.Result,
            NextContext = raw.NextContext,
            Form = raw.Form,
            Sense = raw.Sense,
            Strongs = raw.GreekStrongs.IsNullOrWhiteSpace() ? raw.HebrewStrongs : raw.GreekStrongs,
            Lemma = raw.LemmaHebrew.IsNullOrWhiteSpace() ? raw.LemmaGreek : raw.LemmaHebrew,
            LouwNida = raw.LouwNida,
        };
    }
}