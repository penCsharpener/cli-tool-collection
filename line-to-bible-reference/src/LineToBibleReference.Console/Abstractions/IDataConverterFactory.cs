namespace LineToBibleReference.Console.Abstractions;

public interface IDataConverterFactory
{
    ITextToDataConverter GetDataConverter();
}