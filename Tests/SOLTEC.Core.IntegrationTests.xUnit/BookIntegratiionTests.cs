using OfficeOpenXml;
using SOLTEC.Core.Adapters.Excel;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the Book class using xUnit and EPPlus for mock Excel generation.
/// </summary>
public class BookIntegrationTests
{
    private readonly byte[] _testExcelFile;

    /// <summary>
    /// Sets up an in-memory Excel file using EPPlus with multiple cells populated.
    /// </summary>
    public BookIntegrationTests()
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

    [Fact]
    /// <summary>
    /// Verifies that the Excel stream can be opened successfully.
    /// </summary>
    public void OpenStream_ValidExcelStream_ShouldLoadData()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();

        var response = book.Open(stream);

        Assert.True(response.Success); // Success
    }

    [Fact]
    /// <summary>
    /// Verifies correct decimal cell reading.
    /// </summary>
    public void ReadDecimalCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadDecimalCell(0, "A", 1);
        Assert.Equal(123m, result);
    }

    [Fact]
    /// <summary>
    /// Verifies correct float cell reading.
    /// </summary>
    public void ReadFloatCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadFloatCell(0, "B", 2);
        Assert.True(result.HasValue);
        Assert.Equal(42.5f, (float)result.Value, 1f);
    }

    [Fact]
    /// <summary>
    /// Verifies correct Int32 cell reading.
    /// </summary>
    public void ReadInt32Cell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadInt32Cell(0, "A", 2);
        Assert.Equal(456, result);
    }

    [Fact]
    /// <summary>
    /// Verifies correct Int64 cell reading.
    /// </summary>
    public void ReadInt64Cell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadInt64Cell(0, "E", 2);
        Assert.Equal(9223372036854775807L, result);
    }

    [Fact]
    /// <summary>
    /// Verifies correct DateTime cell reading.
    /// </summary>
    public void ReadDateCell_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadDateCell(0, "F", 2);
        Assert.Equal(DateTime.Today, result);
    }

    [Fact]
    /// <summary>
    /// Verifies cell reading by column letter.
    /// </summary>
    public void ReadCell_ByLetter_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadCell(0, "G", 2);
        Assert.Equal("Hello", result);
    }

    [Fact]
    /// <summary>
    /// Verifies cell reading by column index.
    /// </summary>
    public void ReadCell_ByIndex_ReturnsCorrectValue()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        var result = book.ReadCell(0, 7, 2); // G = 7
        Assert.Equal("Hello", result);
    }

    [Fact]
    /// <summary>
    /// Verifies correct sheet count.
    /// </summary>
    public void GetSheetCount_ReturnsOne()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.Equal(1, book.GetSheetCount());
    }

    [Fact]
    /// <summary>
    /// Verifies correct row count for sheet.
    /// </summary>
    public void GetRowCount_ReturnsTwo()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.Equal(2, book.GetRowCount(0));
    }

    [Fact]
    /// <summary>
    /// Verifies correct column count for sheet.
    /// </summary>
    public void GetColumnCount_ReturnsSeven()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.Equal(7, book.GetColumnCount(0));
    }

    [Fact]
    /// <summary>
    /// Verifies correct sheet name is returned.
    /// </summary>
    public void GetSheetName_ReturnsCorrectName()
    {
        using var stream = new MemoryStream(_testExcelFile);
        var book = new Book();
        book.Open(stream);

        Assert.Equal("Sheet1", book.GetSheetName(0));
    }
}