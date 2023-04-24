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
        nameof(WordMorphologyRawModel.PartOfSpeech),
        nameof(WordMorphologyRawModel.ConjunctionTypeHebrew),
        nameof(WordMorphologyRawModel.EndingTypeHebrew),
        nameof(WordMorphologyRawModel.GenderGreek),
        nameof(WordMorphologyRawModel.NounTypeHebrew),
        nameof(WordMorphologyRawModel.NumberGreek),
        nameof(WordMorphologyRawModel.NumeralTypeHebrew),
        nameof(WordMorphologyRawModel.PersonGreek),
        nameof(WordMorphologyRawModel.PrepositionTypeHebrew),
        nameof(WordMorphologyRawModel.PronounTypeHebrew),
        nameof(WordMorphologyRawModel.StateHebrew),
        nameof(WordMorphologyRawModel.StemHebrew),
        nameof(WordMorphologyRawModel.TAMHebrew),
        nameof(WordMorphologyRawModel.YiqtolVolitivesHebrew),
        nameof(WordMorphologyRawModel.AdverbParticleTypeGreek),
        nameof(WordMorphologyRawModel.CaseGreek),
        nameof(WordMorphologyRawModel.ConjunctionSubtypeGreek),
        nameof(WordMorphologyRawModel.DegreeGreek),
        nameof(WordMorphologyRawModel.GenderHebrew),
        nameof(WordMorphologyRawModel.IndeclinableTypeGreek),
        nameof(WordMorphologyRawModel.MoodGreek),
        nameof(WordMorphologyRawModel.NumberHebrew),
        nameof(WordMorphologyRawModel.PersonHebrew),
        nameof(WordMorphologyRawModel.PronounSubtypeGreek),
        nameof(WordMorphologyRawModel.PronounTypeGreek),
        nameof(WordMorphologyRawModel.TenseGreek),
        nameof(WordMorphologyRawModel.VoiceGreek)
    };

    private static readonly PropertyInfo[] _propertyInfos = typeof(WordMorphologyRawModel).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => relevantPropertyNames.Contains(p.Name)).ToArray();

    public ValueGrouper(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task WriteAllUnitValues(IEnumerable<WordMorphologyRawModel> values)
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

                if (!_unitValues[propName].Contains(value))
                {
                    _unitValues[propName].Add(value);
                }
            }
        }

        var json = JsonSerializer.Serialize(_unitValues, new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

        await _fileService.WriteAllTextToFileAsync("unique-values.json", json);
    }
}
