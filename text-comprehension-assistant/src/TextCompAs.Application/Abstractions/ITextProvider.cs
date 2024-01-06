namespace TextCompAs.Application.Abstractions;

public interface ITextProvider
{
    Task<string> GetTextAsync(CancellationToken token = default);
}
