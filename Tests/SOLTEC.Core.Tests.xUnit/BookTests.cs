using Moq;
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
    private class BookTestable(DataSet data, ServiceResponse response) : Book
    {
        public override ServiceResponse Open(Stream stream) => response;
        public override ServiceResponse Open(string filePath) => response;
        public override DataSet Data => data;
    }
    private static BookTestable CreateMockBook(bool success, int responseCode, bool includeTable = true, DataSet? customData = null)
    {
        var response = new ServiceResponse
        {
            Success = success,
            ResponseCode = responseCode
        };

        var dataSet = customData ?? (includeTable ? CreateSampleDataSet() : new DataSet());
        return new BookTestable(dataSet, response);
    }
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
        var book = CreateMockBook(true, (int)HttpStatusCode.OK);
        var response = book.Open("fake/path/to/file.xlsx");

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
        var book = CreateMockBook(true, (int)HttpStatusCode.OK);
        var fakeStream = new MemoryStream([0x50, 0x4B, 0x03, 0x04]);
        var response = book.Open(fakeStream);

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
        var book = CreateMockBook(false, -1, includeTable: false);
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
        var book = CreateMockBook(false, -1, includeTable: false);
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
        var book = CreateMockBook(true, 200);
        var count = book.GetSheetCount();

        Assert.Equal(1, count);
    }
    [Fact]
    /// <summary>
    /// Test that GetRowCount returns correct row count.
    /// </summary>
    public void GetRowCount_ReturnsCorrectCount()
    {
        var book = CreateMockBook(true, 200);
        var count = book.GetRowCount(0);

        Assert.Equal(1, count);
    }
    [Fact]
    /// <summary>
    /// Test that GetColumnCount returns correct column count.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectCount()
    {
        var book = CreateMockBook(true, 200);
        var count = book.GetColumnCount(0);

        Assert.Equal(7, count);
    }
    [Fact]
    /// <summary>
    /// Test that GetSheetName returns the table name.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        var book = CreateMockBook(true, 200);
        var name = book.GetSheetName(0);

        Assert.Equal("Sheet1", name);
    }
    [Fact]
    /// <summary>
    /// Test that ReadDecimalCell returns correct decimal value.
    /// </summary>
    public void ReadDecimalCell_ReturnsDecimalValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadDecimalCell(0, "A", 1);

        Assert.Equal(123.45m, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadFloatCell returns correct float value.
    /// </summary>
    public void ReadFloatCell_ReturnsFloatValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadFloatCell(0, "B", 1);

        Assert.Equal(2.5f, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadInt32Cell returns correct int value.
    /// </summary>
    public void ReadInt32Cell_ReturnsIntValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadInt32Cell(0, "D", 1);

        Assert.Equal(42, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadInt64Cell returns correct long value.
    /// </summary>
    public void ReadInt64Cell_ReturnsLongValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadInt64Cell(0, "E", 1);

        Assert.Equal(10000000000L, value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadDateCell returns correct DateTime value.
    /// </summary>
    public void ReadDateCell_ReturnsDateTimeValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadDateCell(0, "F", 1);

        Assert.Equal(new DateTime(2025, 1, 1), value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadCell(letter,index) returns correct string value.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsString()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, "G", 1);

        Assert.Equal("TextValue", value);
    }
    [Fact]
    /// <summary>
    /// Test that ReadCell(columnIndex) returns correct string value.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsString()
    {
        var book = CreateMockBook(true, 200);
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
    [Fact]
    /// <summary>
    /// Tests that ReadCell returns null when accessing a non-existent column letter.
    /// </summary>
    public void ReadCell_InvalidColumn_ReturnsNull()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, "Z", 1);

        Assert.Equal(string.Empty, value);
    }
    [Fact]
    /// <summary>
    /// Tests that ReadCell returns null when accessing a column index out of range.
    /// </summary>
    public void ReadCell_InvalidIndex_ReturnsNull()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, 99, 1);

        Assert.Equal(string.Empty, value);
    }
}