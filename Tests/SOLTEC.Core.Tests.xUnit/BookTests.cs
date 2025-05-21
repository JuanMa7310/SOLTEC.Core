using SOLTEC.Core.Adapters.Excel;
using System.Data;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the Book class using xUnit.
/// </summary>
public class BookTests
{
    private readonly Book _book;

    public BookTests()
    {
        _book = new Book();
        var table = new DataTable("Sheet1");
        table.Columns.Add("A");
        table.Columns.Add("B");
        table.Rows.Add("123", "2023-12-31");
        table.Rows.Add("456", "42.5");

        _book.Data.Tables.Add(table);
    }

    /// <summary>
    /// Tests that GetSheetCount returns the correct number of sheets.
    /// </summary>
    [Fact]
    public void GetSheetCount_ReturnsCorrectCount()
    {
        Assert.Equal(1, _book.GetSheetCount());
    }

    /// <summary>
    /// Tests that GetRowCount returns the correct number of rows.
    /// </summary>
    [Fact]
    public void GetRowCount_ReturnsCorrectValue()
    {
        Assert.Equal(2, _book.GetRowCount(0));
    }

    /// <summary>
    /// Tests that GetColumnCount returns the correct number of columns.
    /// </summary>
    [Fact]
    public void GetColumnCount_ReturnsCorrectValue()
    {
        Assert.Equal(2, _book.GetColumnCount(0));
    }

    /// <summary>
    /// Tests that GetSheetName returns the correct name.
    /// </summary>
    [Fact]
    public void GetSheetName_ReturnsCorrectName()
    {
        Assert.Equal("Sheet1", _book.GetSheetName(0));
    }

    /// <summary>
    /// Tests that ReadDecimalCell returns the expected decimal value.
    /// </summary>
    [Fact]
    public void ReadDecimalCell_ValidInput_ReturnsDecimal()
    {
        decimal? result = _book.ReadDecimalCell(0, "A", 1);

        Assert.Equal(123m, result);
    }

    /// <summary>
    /// Tests that ReadInt32Cell returns the expected int value.
    /// </summary>
    [Fact]
    public void ReadInt32Cell_ValidInput_ReturnsInt32()
    {
        int? result = _book.ReadInt32Cell(0, "A", 2);

        Assert.Equal(456, result);
    }

    /// <summary>
    /// Tests that ReadFloatCell returns the expected float value.
    /// </summary>
    [Fact]
    public void ReadFloatCell_ValidInput_ReturnsFloat()
    {
        float? result = _book.ReadFloatCell(0, "B", 2);

        Assert.NotNull(result);
        Assert.Equal(42.5, (double)result!, precision: 1);
    }

    /// <summary>
    /// Tests that ReadDateCell returns the expected DateTime value.
    /// </summary>
    [Fact]
    public void ReadDateCell_ValidDateString_ReturnsDateTime()
    {
        DateTime? result = _book.ReadDateCell(0, "B", 1);

        Assert.Equal(new DateTime(2023, 12, 31), result);
    }

    /// <summary>
    /// Tests that ReadCell by column letter returns the expected string.
    /// </summary>
    [Fact]
    public void ReadCell_ByColumnLetter_ReturnsString()
    {
        string result = _book.ReadCell(0, "A", 1);

        Assert.Equal("123", result);
    }

    /// <summary>
    /// Tests that ReadCell by column index returns the expected string.
    /// </summary>
    [Fact]
    public void ReadCell_ByColumnIndex_ReturnsString()
    {
        string result = _book.ReadCell(0, 1, 1);

        Assert.Equal("123", result);
    }

    /// <summary>
    /// Tests that the private method ColumnToIndex returns the correct index.
    /// </summary>
    [Fact]
    public void ColumnToIndex_ReturnsCorrectIndex()
    {
        var method = typeof(Book).GetMethod("ColumnToIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        Assert.NotNull(method);

        var result = method!.Invoke(null, ["B"]);

        Assert.NotNull(result);

        int index = (int)result;

        Assert.Equal(1, index);
    }
}