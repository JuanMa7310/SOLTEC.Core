using ExcelDataReader;
using Moq;
using SOLTEC.Core.Adapters.Excel;
using System.Data;
using System.Globalization;
using System.Net;
using System.Reflection;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the Book class using NUnit.
/// </summary>
public class BookTests
{
    private Mock<IFileOpener> _fileOpenerMock;
    private Mock<IExcelReaderFactoryWrapper> _readerFactoryMock;
    private Book _book;
    private MemoryStream _dummyStream;
    private Mock<IExcelDataReader> _readerMock;
    private DataSet _sampleDataSet;

    [SetUp]
    public void Setup()
    {
        _fileOpenerMock = new Mock<IFileOpener>();
        _readerFactoryMock = new Mock<IExcelReaderFactoryWrapper>();
        _book = new Book(_fileOpenerMock.Object, _readerFactoryMock.Object);

        // Sample dataset for AsDataSet()
        _sampleDataSet = new DataSet();
        var table = new DataTable("Sheet1");
        table.Columns.Add("A");
        table.Rows.Add("Value");
        _sampleDataSet.Tables.Add(table);

        _dummyStream = new MemoryStream();
        _readerMock = new Mock<IExcelDataReader>();
        _readerMock.Setup(r => r.AsDataSet(It.IsAny<ExcelDataSetConfiguration>()))
                   .Returns(_sampleDataSet);
    }

    [TearDown]
    public void TearDown()
    {
        _dummyStream.Dispose();
        _sampleDataSet.Dispose();
    }

    /// <summary>
    /// Creates a sample DataSet with one sheet and predefined values.
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
        table.Rows.Add("123.45", "2.5", string.Empty, "42", "10000000000", "2025-01-01", "TextValue");
        return new DataSet { Tables = { table } };
    }

    /// <summary>
    /// Sets the Data property of Book via reflection.
    /// </summary>
    private static void SetData(Book book, DataSet dataSet)
    {
        var prop = typeof(Book).GetProperty("Data", BindingFlags.Instance | BindingFlags.Public);
        Assert.That(prop, Is.Not.Null);
        prop!.SetValue(book, dataSet);
    }
    [Test]
    public void Open_WithFilePath_UsesFileOpenerAndReaderFactory()
    {
        // Arrange
        _fileOpenerMock.Setup(op => op.Open("fake.xlsx")).Returns(_dummyStream);
        _readerFactoryMock.Setup(f => f.CreateReader(_dummyStream, It.IsAny<ExcelReaderConfiguration>()))
                          .Returns(_readerMock.Object);

        // Act
        var response = _book.Open("fake.xlsx");

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(response.Success, Is.True);
            Assert.That(response.ResponseCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(_book.Data, Is.EqualTo(_sampleDataSet));
        });
        _fileOpenerMock.Verify(op => op.Open("fake.xlsx"), Times.Once);
        _readerFactoryMock.Verify(f => f.CreateReader(_dummyStream, It.IsAny<ExcelReaderConfiguration>()), Times.Once);
    }

    [Test]
    public void Open_StreamThrowsException_ReturnsError()
    {
        // Arrange
        _readerFactoryMock.Setup(f => f.CreateReader(_dummyStream, It.IsAny<ExcelReaderConfiguration>()))
                          .Throws(new InvalidDataException("Bad format"));
        _fileOpenerMock.Setup(op => op.Open(It.IsAny<string>())).Returns(_dummyStream);

        // Act
        var response = _book.Open("any.xlsx");

        // Assert
        Assert.That(response.Success, Is.False);
        StringAssert.Contains("Bad format", response.ErrorMessage);
    }
    [Test]
    /// <summary>
    /// Test that GetSheetCount returns the correct number of sheets.
    /// </summary>
    public void GetSheetCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var count = book.GetSheetCount();

        Assert.That(count, Is.EqualTo(1));
    }
    [Test]
    /// <summary>
    /// Test that GetRowCount returns the correct number of rows.
    /// </summary>
    public void GetRowCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var rows = book.GetRowCount(0);

        Assert.That(rows, Is.EqualTo(1));
    }
    [Test]
    /// <summary>
    /// Test that GetColumnCount returns the correct number of columns.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectCount()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var cols = book.GetColumnCount(0);

        Assert.That(cols, Is.EqualTo(7));
    }
    [Test]
    /// <summary>
    /// Test that GetSheetName returns the correct sheet name.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var name = book.GetSheetName(0);

        Assert.That(name, Is.EqualTo("Sheet1"));
    }
    [Test]
    /// <summary>
    /// Test that ReadDecimalCell returns the correct decimal value.
    /// </summary>
    public void ReadDecimalCell_ReturnsDecimalValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadDecimalCell(0, "A", 1);

        Assert.That(value, Is.EqualTo(123.45m));
    }
    [Test]
    /// <summary>
    /// Test that ReadFloatCell returns the correct float value.
    /// </summary>
    public void ReadFloatCell_ReturnsFloatValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadFloatCell(0, "B", 1);

        Assert.That(value, Is.EqualTo(2.5f));
    }
    [Test]
    /// <summary>
    /// Test that ReadInt32Cell returns the correct integer value.
    /// </summary>
    public void ReadInt32Cell_ReturnsIntValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadInt32Cell(0, "D", 1);

        Assert.That(value, Is.EqualTo(42));
    }
    [Test]
    /// <summary>
    /// Test that ReadInt64Cell returns the correct long value.
    /// </summary>
    public void ReadInt64Cell_ReturnsLongValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadInt64Cell(0, "E", 1);

        Assert.That(value, Is.EqualTo(10000000000L));
    }
    [Test]
    /// <summary>
    /// Test that ReadDateCell returns the correct DateTime value.
    /// </summary>
    public void ReadDateCell_ReturnsDateTimeValue()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadDateCell(0, "F", 1);

        Assert.That(value, Is.EqualTo(new DateTime(2025, 1, 1)));
    }
    [Test]
    /// <summary>
    /// Test that ReadCell by column letter returns the correct string value.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsString()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadCell(0, "G", 1);

        Assert.That(value, Is.EqualTo("TextValue"));
    }
    [Test]
    /// <summary>
    /// Test that ReadCell by column index returns the correct string value.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsString()
    {
        var book = new Book();
        var sample = CreateSampleDataSet();
        SetData(book, sample);
        var value = book.ReadCell(0, 7, 1);

        Assert.That(value, Is.EqualTo("TextValue"));
    }
    [Test]
    /// <summary>
    /// Test that ColumnToIndex computes the correct index for multi-letter columns.
    /// </summary>
    public void ColumnToIndex_ComputesCorrectValue()
    {
        var method = typeof(Book).GetMethod("ColumnToIndex", BindingFlags.Static | BindingFlags.NonPublic);
        var result = method!.Invoke(null, ["AB"]);
        var index = Convert.ToInt32(result);

        Assert.Multiple(() =>
        {
            Assert.That(method, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
            Assert.That(index, Is.EqualTo(27));
        });
    }
}