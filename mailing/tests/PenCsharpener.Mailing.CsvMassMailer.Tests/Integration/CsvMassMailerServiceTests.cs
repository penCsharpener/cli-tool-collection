using PenCsharpener.Mailing.CsvMassMailer.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Tests.Integration;

public class CsvMassMailerServiceTests
{
    private readonly ICsvMassMailerService _testObject;

    public CsvMassMailerServiceTests(ICsvMassMailerService testObject)
    {
        _testObject = testObject;
    }

    [Fact]
    public async Task ExecuteAsync_Writes_Correct_Emails_As_TextFiles()
    {
        await _testObject.ExecuteAsync(new());
    }
}
