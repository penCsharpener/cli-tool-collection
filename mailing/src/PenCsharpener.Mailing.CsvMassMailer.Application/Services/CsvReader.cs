using PenCsharpener.Mailing.CsvMassMailer.Application.Models;
using PenCsharpener.Mailing.CsvMassMailer.Application.Services.Abstractions;

namespace PenCsharpener.Mailing.CsvMassMailer.Application.Services;

public class CsvReader : ICsvReader
{
    public CsvReader()
    {

    }

    public CsvContent? ParseCsvContent(string[] csvLines)
    {
        var tableCells = GetTableCells(csvLines).ToArray();

        var replacementColumns = tableCells.Where(tc => tc.IsHeaderCell && tc.Value.StartsWith("{{") && tc.Value.EndsWith("}}")).ToArray();
        var replacementColumnNumbers = replacementColumns.Select(rc => rc.ColumnIndex).ToArray();

        var dict = new Dictionary<string, IList<ReplacementPair>>();

        for (var i = 1; i < csvLines.Length; i++)
        {
            var recipient = tableCells.First(tc => tc.RowIndex == i && tc.ColumnIndex == 0).Value;

            dict.Add(recipient, new List<ReplacementPair>());

            var rowReplacementPairs = tableCells.Where(tc => tc.RowIndex == i && replacementColumnNumbers.Contains(tc.ColumnIndex)).Select(tc => new ReplacementPair(replacementColumns.First(rc => rc.ColumnIndex == tc.ColumnIndex).Value, tc.Value));

            foreach (var pairs in rowReplacementPairs)
            {
                dict[recipient].Add(pairs);
            }
        }

        var csvContent = new CsvContent()
        {
            EmailRecipients = dict.Keys.ToArray(),
            ReplacementTokens = dict
        };

        return csvContent;
    }

    private IEnumerable<TableCell<string>> GetTableCells(string[] csvLines)
    {
        for (var rowIndex = 0; rowIndex < csvLines.Length; rowIndex++)
        {
            var columnsCells = csvLines[rowIndex].Split(',').ToArray();

            for (var columnIndex = 0; columnIndex < columnsCells.Length; columnIndex++)
            {
                yield return new(rowIndex, columnIndex, columnsCells[columnIndex]);
            }
        }
    }
}
