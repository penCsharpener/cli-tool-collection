using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Extensions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public sealed class MorphReader : IMorphReader
{
    private readonly IFileService _fileService;
    private readonly AppSettings _settings;

    public MorphReader(IFileService fileService, AppSettings settings)
    {
        _fileService = fileService;
        _settings = settings;
    }

    public async IAsyncEnumerable<WordMorphologyModel> ReadMorphologyAsync()
    {
        var pathSettings = _settings.CsvPathMapping["hebl"];

        foreach (var filePath in _fileService.GetFilesInDirectory(pathSettings.Path, pathSettings.FileFilter, new Regex(pathSettings.RegexFilter)))
        {
            using var reader = new StreamReader(filePath.FullName);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            {
                csv.Context.RegisterClassMap<PropertyMap>();

                await foreach (var item in csv.GetRecordsAsync<WordMorphologyRawModel>())
                {
                    yield return item.ToWordMorphologyModel();
                }
            }
        }
    }

    public sealed class PropertyMap : ClassMap<WordMorphologyRawModel>
    {
        public PropertyMap()
        {
            Map(m => m.Resource);
            Map(m => m.Book);
            Map(m => m.Chapter);
            Map(m => m.Reference);
            Map(m => m.PreviousContext).Name("Previous Context");
            Map(m => m.Result).Name("Result");
            Map(m => m.NextContext).Name("Next Context");
            Map(m => m.Form);
            Map(m => m.HebrewStrongs).Name("Hebrew Strong’s").Optional();
            Map(m => m.LemmaHebrew).Name("Lemma (Hebrew)").Optional();
            Map(m => m.Root).Name("Root");
            Map(m => m.Sense).Name("Sense");
            Map(m => m.PartOfSpeech).Name("Part of Speech");
            Map(m => m.ConjunctionTypeHebrew).Name("Conjunction Type (Logos Hebrew)").Optional();
            Map(m => m.EndingTypeHebrew).Name("Ending Type (Logos Hebrew)").Optional();
            Map(m => m.GenderHebrew).Name("Gender (Logos Hebrew)").Optional();
            Map(m => m.NounTypeHebrew).Name("Noun Type (Logos Hebrew)").Optional();
            Map(m => m.NumberHebrew).Name("Number (Logos Hebrew)").Optional();
            Map(m => m.NumeralTypeHebrew).Name("Numeral Type (Logos Hebrew)").Optional();
            Map(m => m.PersonHebrew).Name("Person (Logos Hebrew)").Optional();
            Map(m => m.PrepositionTypeHebrew).Name("Preposition Type (Logos Hebrew)").Optional();
            Map(m => m.PronounTypeHebrew).Name("Pronoun Type (Logos Hebrew)").Optional();
            Map(m => m.StateHebrew).Name("State (Logos Hebrew)").Optional();
            Map(m => m.StemHebrew).Name("Stem (Logos Hebrew)").Optional();
            Map(m => m.TAMHebrew).Name("TAM (Logos Hebrew)").Optional();
            Map(m => m.YiqtōlVolitivesHebrew).Name("Yiqtōl Volitives (Logos Hebrew)").Optional();
            Map(m => m.GreekStrongs).Name("Greek Strong’s").Optional();
            Map(m => m.LemmaGreek).Name("Lemma (Greek)").Optional();
            Map(m => m.LouwNida).Name("Louw-Nida").Optional();
            Map(m => m.AdverbParticleTypeGreek).Name("Adverb/particle Type (Logos Greek)").Optional();
            Map(m => m.CaseGreek).Name("Case (Logos Greek)").Optional();
            Map(m => m.ConjunctionSubtypeGreek).Name("Conjunction Subtype (Logos Greek)").Optional();
            Map(m => m.DegreeGreek).Name("Degree (Logos Greek)").Optional();
            Map(m => m.GenderGreek).Name("Gender (Logos Greek)").Optional();
            Map(m => m.IndeclinableTypeGreek).Name("Indeclinable Type (Logos Greek)").Optional();
            Map(m => m.MoodGreek).Name("Mood (Logos Greek)").Optional();
            Map(m => m.NumberGreek).Name("Number (Logos Greek)").Optional();
            Map(m => m.PersonGreek).Name("Person (Logos Greek)").Optional();
            Map(m => m.PronounSubtypeGreek).Name("Pronoun Subtype (Logos Greek)").Optional();
            Map(m => m.PronounTypeGreek).Name("Pronoun Type (Logos Greek)").Optional();
            Map(m => m.TenseGreek).Name("Tense (Logos Greek)").Optional();
            Map(m => m.VoiceGreek).Name("Voice (Logos Greek)").Optional();
        }
    }
}