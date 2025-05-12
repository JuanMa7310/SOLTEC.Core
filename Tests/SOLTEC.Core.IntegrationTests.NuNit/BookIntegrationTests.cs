using NUnit.Framework;
using OfficeOpenXml;
using SOLTEC.Core.Adapters.Excel;

namespace SOLTEC.Core.IntegrationTests.NuNit;

[TestFixture]
/// <summary>
/// Integration tests for the Book class using NUnit and EPPlus.
/// </summary>
public class BookIntegrationTests
{
    private byte[] _testExcelFile = Array.Empty<byte>();

    [SetUp]
    /// <summary>
    /// Initializes a test Excel file in memory using EPPlus.
    /// </summary>
    public void Setup()
    {
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Sheet1");

        sheet.Cells["A1"].Value = "123";
        sheet.Cells["B1"].Value = "2023-12-31";
        sheet.Cells["A2"].Value = "456";
        sheet.Cells["B2"].Value = "42.5";
        sheet.Cells["C2"].Value = "99.9";
        sheet.Cells["D2"].Value = "789";
        sheet.Cells["E2"].Value = "9223372036854775807";
        sheet.Cells["F2"].Value = DateTime.Today;
        sheet.Cells["G2"].Value = "Hello";

        _testExcelFile = package.GetAsByteArray();
    }

    [Test]
    /// <summary>
    /// Tests that a valid Excel stream loads data successfully.
    /// </summary>
    public void OpenStream_ValidExcelStream_ShouldLoadData()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();

        var response = book.Open(stream);

        Assert.That(response.Success, Is.EqualTo(0));
    }

    [Test]
    /// <summary>
    /// Tests reading a decimal value from Excel.
    /// </summary>
    public void ReadDecimalCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadDecimalCell(0, "A", 1);

        Assert.That(result, Is.EqualTo(123m));
    }

    [Test]
    /// <summary>
    /// Tests reading a float value from Excel.
    /// </summary>
    public void ReadFloatCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadFloatCell(0, "B", 2);

        Assert.That(result, Is.EqualTo(42.5f).Within(0.1f));
    }

    [Test]
    /// <summary>
    /// Tests reading an Int32 value from Excel.
    /// </summary>
    public void ReadInt32Cell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadInt32Cell(0, "A", 2);

        Assert.That(result, Is.EqualTo(456));
    }

    [Test]
    /// <summary>
    /// Tests reading an Int64 value from Excel.
    /// </summary>
    public void ReadInt64Cell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadInt64Cell(0, "E", 2);

        Assert.That(result, Is.EqualTo(9223372036854775807L));
    }

    [Test]
    /// <summary>
    /// Tests reading a DateTime value from Excel.
    /// </summary>
    public void ReadDateCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadDateCell(0, "F", 2);

        Assert.That(result, Is.EqualTo(DateTime.Today));
    }

    [Test]
    /// <summary>
    /// Tests reading a string cell by column letter.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadCell(0, "G", 2);

        Assert.That(result, Is.EqualTo("Hello"));
    }

    [Test]
    /// <summary>
    /// Tests reading a string cell by column index.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadCell(0, 7, 2); // G = 7

        Assert.That(result, Is.EqualTo("Hello"));
    }

    [Test]
    /// <summary>
    /// Tests the number of sheets in the workbook.
    /// </summary>
    public void GetSheetCount_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.That(book.GetSheetCount(), Is.EqualTo(1));
    }

    [Test]
    /// <summary>
    /// Tests the number of rows in the first sheet.
    /// </summary>
    public void GetRowCount_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.That(book.GetRowCount(0), Is.EqualTo(2));
    }

    [Test]
    /// <summary>
    /// Tests the number of columns in the first sheet.
    /// </summary>
    public void GetColumnCount_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.That(book.GetColumnCount(0), Is.EqualTo(7));
    }

    [Test]
    /// <summary>
    /// Tests the name of the first sheet.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.That(book.GetSheetName(0), Is.EqualTo("Sheet1"));
    }
}