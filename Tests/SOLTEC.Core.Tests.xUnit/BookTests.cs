using SOLTEC.Core.Adapters.Excel;
using System.Data;
using System.Net;
using System.Reflection;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the Book class using xUnit.
/// </summary>
public class BookTests
{
    private const string SampleFileName = "TestData/Sample.xlsx";

    private static DataSet CreateSampleDataSet()
    {
        var table = new DataTable("Sheet1");

        table.Columns.Add("A");
        table.Columns.Add("B");
        table.Columns.Add("C");
        table.Columns.Add("D");
        table.Columns.Add("E");
        table.Columns.Add("F");
        table.Columns.Add("G");
        table.Rows.Add("123.45", "2.5", "", "42", "10000000000", "2025-01-01", "TextValue");
        return new DataSet { Tables = { table } };
    }

    private static void SetData(Book book, DataSet dataSet)
    {
        var property = typeof(Book).GetProperty("Data", BindingFlags.Instance | BindingFlags.Public);

        property?.SetValue(book, dataSet);
    }

    [Fact]
    /// <summary>
    /// Test that Open(file path) returns success for a valid Excel file.
    /// </summary>
    public void Open_WithFilePath_ReturnsSuccess()
    {
        var book = new Book();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SampleFileName);
        var response = book.Open(path);

        Assert.True(response.Success);
        Assert.Equal((int)HttpStatusCode.OK, response.ResponseCode);
        Assert.NotEmpty(book.Data.Tables);
    }
    [Fact]
    /// <summary>
    /// Test that Open(stream) returns success for a valid Excel stream.
    /// </summary>
    public void Open_WithStream_ReturnsSuccess()
    {
        var book = new Book();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SampleFileName);
        using var stream = File.OpenRead(path);
        var response = book.Open(stream);

        Assert.True(response.Success);
        Assert.Equal((int)HttpStatusCode.OK, response.ResponseCode);
        Assert.NotEmpty(book.Data.Tables);
    }
    [Fact]
    /// <summary>
    /// Test that Open(file path) returns error for an invalid file path.
    /// </summary>
    public void Open_WithInvalidFilePath_ReturnsError()
    {
        var book = new Book();
        var response = book.Open("nonexistent.xlsx");

        Assert.False(response.Success);
        Assert.Equal(-1, response.ResponseCode);
    }
    [Fact]
    /// <summary>
    /// Test that Open(stream) returns error for an invalid stream.
    /// </summary>
    public void Open_WithInvalidStream_ReturnsError()
    {
        var book = new Book();
        using var invalidStream = new MemoryStream([0x00]);
        var response = book.Open(invalidStream);

        Assert.False(response.Success);
        Assert.Equal(-1, response.ResponseCode);
    }
    [Fact]
    /// <summary>
    /// Test that GetSheetCount returns correct table count.
    /// </summary>
    public void GetSheetCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var count = book.GetSheetCount();

        Assert.Equal(1, count);
    }
    [Fact]
    /// <summary>
    /// Test that GetRowCount returns correct row count.
    /// </summary>
    public void GetRowCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var rows = book.GetRowCount(0);

        Assert.Equal(1, rows);
    }
    [Fact]
    /// <summary>
    /// Test that GetColumnCount returns correct column count.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var cols = book.GetColumnCount(0);

        Assert.Equal(7, cols);
    }
    [Fact]
    /// <summary>
    /// Test that GetSheetName returns the table name.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var name = book.GetSheetName(0);

        Assert.Equal("Sheet1", name);
    }
    [Fact]
    /// <summary>
    /// Test that ReadDecimalCell returns correct decimal value.
    /// </summary>
    public void ReadDecimalCell_ReturnsDecimalValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadDecimalCell(0, "A", 1);

        Assert.Equal(123.45m, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadFloatCell returns correct float value.
    /// </summary>
    public void ReadFloatCell_ReturnsFloatValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadFloatCell(0, "B", 1);

        Assert.Equal(2.5f, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadInt32Cell returns correct int value.
    /// </summary>
    public void ReadInt32Cell_ReturnsIntValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadInt32Cell(0, "D", 1);

        Assert.Equal(42, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadInt64Cell returns correct long value.
    /// </summary>
    public void ReadInt64Cell_ReturnsLongValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadInt64Cell(0, "E", 1);

        Assert.Equal(10000000000L, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadDateCell returns correct DateTime value.
    /// </summary>
    public void ReadDateCell_ReturnsDateTimeValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadDateCell(0, "F", 1);

        Assert.Equal(new DateTime(2025, 1, 1), value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadCell(letter,index) returns correct string value.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsString()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadCell(0, "G", 1);

        Assert.Equal("TextValue", value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadCell(columnIndex) returns correct string value.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsString()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadCell(0, 7, 1);

        Assert.Equal("TextValue", value);
    }
    [Fact]
    /// <summary>
    /// Test that ColumnToIndex computes correct index for multiletter.
    /// </summary>
    public void ColumnToIndex_ComputesCorrectValue()
    {
        var method = typeof(Book).GetMethod("ColumnToIndex", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.NotNull(method);
        var resultObj = method!.Invoke(null, ["AB"]);
        Assert.NotNull(resultObj);
        var index = (int)resultObj!;

        Assert.Equal(27, index);
    }
}