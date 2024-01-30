namespace SemiAutoTranslator.Console.Abstractions;
public interface ITextDividerService
{
    public IAsyncEnumerable<string> GetTextSnippet(CancellationToken token = default);
}
