namespace PenCsharpener.Mailing.CsvMassMailer.Application.Models;

public struct TableCell<T>
{
    public TableCell(int rowIndex, int columnIndex, T value)
    {
        RowIndex = rowIndex;
        ColumnIndex = columnIndex;
        Value = value;
    }

    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public T Value { get; set; }

    public bool IsHeaderCell => RowIndex == 0;
}