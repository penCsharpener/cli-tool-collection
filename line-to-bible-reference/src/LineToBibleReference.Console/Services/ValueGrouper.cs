using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using LineToBibleReference.Console.Abstractions;
using LineToBibleReference.Console.Models;

namespace LineToBibleReference.Console.Services;

public sealed class ValueGrouper : IValueGrouper
{
    private readonly IFileService _fileService;
    private readonly Dictionary<string, List<string>> _unitValues = new();

    private static readonly string[] relevantPropertyNames = new[] {
        nameof(WordMorphologyModel.PartOfSpeech),
        nameof(WordMorphologyModel.ConjunctionTypeHebrew),
        nameof(WordMorphologyModel.EndingTypeHebrew),
        nameof(WordMorphologyModel.GenderHebrew),
        nameof(WordMorphologyModel.NounTypeHebrew),
        nameof(WordMorphologyModel.NumberHebrew),
        nameof(WordMorphologyModel.NumeralTypeHebrew),
        nameof(WordMorphologyModel.PersonHebrew),
        nameof(WordMorphologyModel.PrepositionTypeHebrew),
        nameof(WordMorphologyModel.PronounTypeHebrew),
        nameof(WordMorphologyModel.StateHebrew),
        nameof(WordMorphologyModel.StemHebrew),
        nameof(WordMorphologyModel.TAMHebrew),
        nameof(WordMorphologyModel.YiqtōlVolitivesHebrew),
        nameof(WordMorphologyModel.AdverbParticleTypeGreek),
        nameof(WordMorphologyModel.CaseGreek),
        nameof(WordMorphologyModel.ConjunctionSubtypeGreek),
        nameof(WordMorphologyModel.DegreeGreek),
        nameof(WordMorphologyModel.GenderGreek),
        nameof(WordMorphologyModel.IndeclinableTypeGreek),
        nameof(WordMorphologyModel.MoodGreek),
        nameof(WordMorphologyModel.NumberGreek),
        nameof(WordMorphologyModel.PersonGreek),
        nameof(WordMorphologyModel.PronounSubtypeGreek),
        nameof(WordMorphologyModel.PronounTypeGreek),
        nameof(WordMorphologyModel.TenseGreek),
        nameof(WordMorphologyModel.VoiceGreek)
    };

    private static readonly PropertyInfo[] _propertyInfos = typeof(WordMorphologyModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => relevantPropertyNames.Contains(p.Name)).ToArray();

    public ValueGrouper(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task WriteAllUnitValues(IEnumerable<WordMorphologyModel> values)
    {
        foreach (var v in values)
        {
            foreach (var propName in relevantPropertyNames)
            {
                var value = (string?)_propertyInfos.First(pi => pi.Name == propName).GetValue(v);

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (!_unitValues.ContainsKey(propName))
                {
                    _unitValues[propName] = value.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();

                    continue;
                }

                foreach (var item in value.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()))
                {
                    if (!_unitValues[propName].Contains(item))
                    {
                        _unitValues[propName].Add(item);
                    }
                }
            }
        }

        var json = JsonSerializer.Serialize(_unitValues, new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

        await _fileService.WriteAllTextToFileAsync("unique-values.json", json);
    }
}
