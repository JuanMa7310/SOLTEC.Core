using SOLTEC.Core.Adapters.Excel;
using System.Data;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the Book class using NUnit.
/// </summary>
public class BookTests
{
    private Book _book;

    [SetUp]
    public void Setup()
    {
        _book = new Book();
        var table = new DataTable("Sheet1");
        table.Columns.Add("A");
        table.Columns.Add("B");
        table.Rows.Add("123", "2023-12-31");
        table.Rows.Add("456", "42.5");

        _book.Data.Tables.Add(table);
    }

    [Test]
    /// <summary>
    /// Tests that GetSheetCount returns the correct number of sheets.
    /// </summary>
    public void GetSheetCount_ReturnsCorrectCount()
    {
        Assert.That(_book.GetSheetCount(), Is.EqualTo(1));
    }

    [Test]
    /// <summary>
    /// Tests that GetRowCount returns the correct number of rows.
    /// </summary>
    public void GetRowCount_ReturnsCorrectValue()
    {
        Assert.That(_book.GetRowCount(0), Is.EqualTo(2));
    }

    [Test]
    /// <summary>
    /// Tests that GetColumnCount returns the correct number of columns.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectValue()
    {
        Assert.That(_book.GetColumnCount(0), Is.EqualTo(2));
    }

    [Test]
    /// <summary>
    /// Tests that GetSheetName returns the correct name.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        Assert.That(_book.GetSheetName(0), Is.EqualTo("Sheet1"));
    }

    [Test]
    /// <summary>
    /// Tests that ReadDecimalCell returns the expected decimal value.
    /// </summary>
    public void ReadDecimalCell_ValidInput_ReturnsDecimal()
    {
        decimal? result = _book.ReadDecimalCell(0, "A", 1);

        Assert.That(result, Is.EqualTo(123m));
    }

    [Test]
    /// <summary>
    /// Tests that ReadInt32Cell returns the expected int value.
    /// </summary>
    public void ReadInt32Cell_ValidInput_ReturnsInt32()
    {
        int? result = _book.ReadInt32Cell(0, "A", 2);

        Assert.That(result, Is.EqualTo(456));
    }

    [Test]
    /// <summary>
    /// Tests that ReadFloatCell returns the expected float value.
    /// </summary>
    public void ReadFloatCell_ValidInput_ReturnsFloat()
    {
        float? result = _book.ReadFloatCell(0, "B", 2);

        Assert.That(result, Is.EqualTo(42.5f).Within(0.1f));
    }

    [Test]
    /// <summary>
    /// Tests that ReadDateCell returns the expected DateTime value.
    /// </summary>
    public void ReadDateCell_ValidDateString_ReturnsDateTime()
    {
        DateTime? result = _book.ReadDateCell(0, "B", 1);

        Assert.That(result, Is.EqualTo(new DateTime(2023, 12, 31)));
    }

    [Test]
    /// <summary>
    /// Tests that ReadCell by column letter returns the expected string.
    /// </summary>
    public void ReadCell_ByColumnLetter_ReturnsString()
    {
        string result = _book.ReadCell(0, "A", 1);

        Assert.That(result, Is.EqualTo("123"));
    }

    [Test]
    /// <summary>
    /// Tests that ReadCell by column index returns the expected string.
    /// </summary>
    public void ReadCell_ByColumnIndex_ReturnsString()
    {
        string result = _book.ReadCell(0, 1, 1);

        Assert.That(result, Is.EqualTo("123"));
    }

    [Test]
    /// <summary>
    /// Tests that the private method ColumnToIndex returns the correct index.
    /// </summary>
    public void ColumnToIndex_ReturnsCorrectIndex()
    {
        var method = typeof(Book).GetMethod("ColumnToIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        Assert.That(method, Is.Not.Null, "The method 'ColumnToIndex' was not found via reflection.");

        var result = method?.Invoke(null, ["B"]);

        Assert.That(result, Is.Not.Null);

        int index = (int)result!;

        Assert.That(index, Is.EqualTo(1));
    }
}
