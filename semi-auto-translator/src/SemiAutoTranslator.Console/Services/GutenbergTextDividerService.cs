using System.Runtime.CompilerServices;
using SemiAutoTranslator.Console.Abstractions;
using SemiAutoTranslator.Console.Models;

namespace SemiAutoTranslator.Console.Services;

public class GutenbergTextDividerService : ITextDividerService
{
    private readonly AppSettings _appSettings;
    private readonly IFileService _fileService;
    private const string SentenceDelimiter = "|||";

    public GutenbergTextDividerService(AppSettings appSettings, IFileService fileService)
    {
        _appSettings = appSettings;
        _fileService = fileService;
    }

    public async IAsyncEnumerable<string> GetTextSnippet([EnumeratorCancellation] CancellationToken token = default)
    {
        var translatableText = string.Empty;

        await foreach (var textBlock in _fileService.GetLinesInFile(token))
        {
            var sentences = textBlock.Replace(". ", $". {SentenceDelimiter}").Replace("! ", $"! {SentenceDelimiter}").Replace("? ", $"? {SentenceDelimiter}")
                .Replace(".\" ", $".\" {SentenceDelimiter}").Replace("!\" ", $"!\" {SentenceDelimiter}").Replace("?\" ", $"?\" {SentenceDelimiter}")
                .Replace($"Mr. {SentenceDelimiter}", "Mr. ").Replace($"Mrs. {SentenceDelimiter}", "")
                .Split(SentenceDelimiter, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < sentences.Length; i++)
            {
                var sentence = sentences[i];

                if (translatableText.Length + sentence.Length > _appSettings.MaxCharactersPerRequest)
                {
                    var returnValue = translatableText;

                    translatableText = sentence;

                    yield return returnValue;

                    continue;
                }

                translatableText += sentence + "·";
            }
        }

        yield return translatableText;
    }
}
