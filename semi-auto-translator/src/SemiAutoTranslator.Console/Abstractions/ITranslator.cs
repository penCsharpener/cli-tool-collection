namespace SemiAutoTranslator.Console.Abstractions;
public interface ITranslator
{
    Task TranslateAsync(CancellationToken token = default);
}
