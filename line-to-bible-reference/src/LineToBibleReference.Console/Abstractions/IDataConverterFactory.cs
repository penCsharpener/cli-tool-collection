namespace LineToBibleReference.Console.Abstractions;

public interface IDataConverterFactory
{
    ITextToDataConverter GetDataConverter(string converterType);
}

public interface IDiacriticRemovalServiceFactory
{
    IDiacriticRemovalService GetDiacriticRemovalService(string converterType);
}

public interface IDiacriticRemovalService
{
    string RemoveDiacritics(string word);
    IEnumerable<string> RemoveDiacritics(IEnumerable<string> word);
}
