using SOLTEC.Core.Adapters.Excel;
using System.Data;
using System.Net;
using System.Reflection;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the Book class using NUnit.
/// </summary>
public class BookTests
{
    /// <summary>
    /// A testable implementation of the Book class that overrides Open methods and exposes Data.
    /// </summary>
    private class BookTestable(DataSet data, ServiceResponse response) : Book
    {
        public override ServiceResponse Open(Stream stream) => response;
        public override ServiceResponse Open(string filePath) => response;
        public override DataSet Data => data;
    }

    /// <summary>
    /// Creates a mock instance of the BookTestable class.
    /// </summary>
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

    /// <summary>
    /// Creates a sample DataSet with one table and one row of example data.
    /// </summary>
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

    /// <summary>
    /// Uses reflection to inject a custom DataSet into a Book instance.
    /// </summary>
    private static void SetData(Book book, DataSet dataSet)
    {
        var property = typeof(Book).GetProperty("Data", BindingFlags.Instance | BindingFlags.Public);
        property?.SetValue(book, dataSet);
    }

    [Test]
    /// <summary>
    /// Test that Open(file path) returns success for a valid Excel file.
    /// </summary>
    public void Open_WithFilePath_ReturnsSuccess()
    {
        var book = CreateMockBook(true, (int)HttpStatusCode.OK);
        var response = book.Open("fake/path/to/file.xlsx");

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.ResponseCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(book.Data.Tables, Is.Not.Empty);
        });
    }

    [Test]
    /// <summary>
    /// Test that Open(stream) returns success for a valid Excel stream.
    /// </summary>
    public void Open_WithStream_ReturnsSuccess()
    {
        var book = CreateMockBook(true, (int)HttpStatusCode.OK);
        using var fakeStream = new MemoryStream([0x50, 0x4B, 0x03, 0x04]);
        var response = book.Open(fakeStream);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.True);
            Assert.That(response.ResponseCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(book.Data.Tables, Is.Not.Empty);
        });
    }

    [Test]
    /// <summary>
    /// Test that Open(file path) returns error for an invalid file path.
    /// </summary>
    public void Open_WithInvalidFilePath_ReturnsError()
    {
        var book = CreateMockBook(false, -1, includeTable: false);
        var response = book.Open("nonexistent.xlsx");

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.ResponseCode, Is.EqualTo(-1));
        });
    }

    [Test]
    /// <summary>
    /// Test that Open(stream) returns error for an invalid stream.
    /// </summary>
    public void Open_WithInvalidStream_ReturnsError()
    {
        var book = CreateMockBook(false, -1, includeTable: false);
        using var invalidStream = new MemoryStream([0x00]);
        var response = book.Open(invalidStream);

        Assert.Multiple(() =>
        {
            Assert.That(response.Success, Is.False);
            Assert.That(response.ResponseCode, Is.EqualTo(-1));
        });
    }

    [Test]
    /// <summary>
    /// Test that GetSheetCount returns correct table count.
    /// </summary>
    public void GetSheetCount_ReturnsCorrectCount()
    {
        var book = CreateMockBook(true, 200);
        var count = book.GetSheetCount();

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    /// <summary>
    /// Test that GetRowCount returns correct row count.
    /// </summary>
    public void GetRowCount_ReturnsCorrectCount()
    {
        var book = CreateMockBook(true, 200);
        var count = book.GetRowCount(0);

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    /// <summary>
    /// Test that GetColumnCount returns correct column count.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectCount()
    {
        var book = CreateMockBook(true, 200);
        var count = book.GetColumnCount(0);

        Assert.That(count, Is.EqualTo(7));
    }

    [Test]
    /// <summary>
    /// Test that GetSheetName returns the table name.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        var book = CreateMockBook(true, 200);
        var name = book.GetSheetName(0);

        Assert.That(name, Is.EqualTo("Sheet1"));
    }

    [Test]
    /// <summary>
    /// Test that ReadDecimalCell returns correct decimal value.
    /// </summary>
    public void ReadDecimalCell_ReturnsDecimalValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadDecimalCell(0, "A", 1);

        Assert.That(value, Is.EqualTo(123.45m));
    }

    [Test]
    /// <summary>
    /// Test that ReadFloatCell returns correct float value.
    /// </summary>
    public void ReadFloatCell_ReturnsFloatValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadFloatCell(0, "B", 1);

        Assert.That(value, Is.EqualTo(2.5f));
    }

    [Test]
    /// <summary>
    /// Test that ReadInt32Cell returns correct int value.
    /// </summary>
    public void ReadInt32Cell_ReturnsIntValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadInt32Cell(0, "D", 1);

        Assert.That(value, Is.EqualTo(42));
    }

    [Test]
    /// <summary>
    /// Test that ReadInt64Cell returns correct long value.
    /// </summary>
    public void ReadInt64Cell_ReturnsLongValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadInt64Cell(0, "E", 1);

        Assert.That(value, Is.EqualTo(10000000000L));
    }

    [Test]
    /// <summary>
    /// Test that ReadDateCell returns correct DateTime value.
    /// </summary>
    public void ReadDateCell_ReturnsDateTimeValue()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadDateCell(0, "F", 1);

        Assert.That(value, Is.EqualTo(new DateTime(2025, 1, 1)));
    }

    [Test]
    /// <summary>
    /// Test that ReadCell(letter,index) returns correct string value.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsString()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, "G", 1);

        Assert.That(value, Is.EqualTo("TextValue"));
    }

    [Test]
    /// <summary>
    /// Test that ReadCell(columnIndex) returns correct string value.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsString()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, 7, 1);

        Assert.That(value, Is.EqualTo("TextValue"));
    }

    [Test]
    /// <summary>
    /// Test that ColumnToIndex computes correct index for multiletter.
    /// </summary>
    public void ColumnToIndex_ComputesCorrectValue()
    {
        var method = typeof(Book).GetMethod("ColumnToIndex", BindingFlags.Static | BindingFlags.NonPublic);
        Assert.That(method, Is.Not.Null);
        var resultObj = method!.Invoke(null, ["AB"]);
        Assert.That(resultObj, Is.Not.Null);
        var index = (int)resultObj!;

        Assert.That(index, Is.EqualTo(27));
    }

    [Test]
    /// <summary>
    /// Tests that ReadCell returns empty string when accessing a non-existent column letter.
    /// </summary>
    public void ReadCell_InvalidColumn_ReturnsNull()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, "Z", 1);

        Assert.That(value, Is.EqualTo(string.Empty));
    }

    [Test]
    /// <summary>
    /// Tests that ReadCell returns empty string when accessing a column index out of range.
    /// </summary>
    public void ReadCell_InvalidIndex_ReturnsNull()
    {
        var book = CreateMockBook(true, 200);
        var value = book.ReadCell(0, 99, 1);

        Assert.That(value, Is.EqualTo(string.Empty));
    }
}