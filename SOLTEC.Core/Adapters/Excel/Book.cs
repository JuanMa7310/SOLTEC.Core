using ExcelDataReader;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;

namespace SOLTEC.Core.Adapters.Excel;

/// <summary>
/// Provides functionality to open and read Excel files.
/// </summary>
/// <example>
/// <![CDATA[
/// var book = new Book();
/// var result = book.Open("path\to\file.xlsx");
/// if (result.IsSuccess)
/// {
///     int sheets = book.GetSheetCount();
///     string cell = book.ReadCell(0, "A", 1);
/// }
/// ]]>
/// </example>
/// <remarks>
/// Creates a new instance of <see cref="Book"/> with injected dependencies.
/// </remarks>
/// <param name="fileOpener">The file opener to use for opening streams.</param>
/// <param name="readerFactory">The reader factory to use for creating Excel readers.</param>
/// <example>
/// <![CDATA[
/// var fileOpener = new DefaultFileOpener();
/// var readerFactory = new ExcelReaderFactoryWrapper();
/// var book = new Book(fileOpener, readerFactory);
/// ]]>
/// </example>
public class Book(IFileOpener fileOpener, IExcelReaderFactoryWrapper readerFactory)
{

    /// <summary>
    /// Creates a new instance of <see cref="Book"/> with default file opener and reader factory.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// var book = new Book();
    /// ]]>
    /// </example>
    public Book() : this(new DefaultFileOpener(), new ExcelReaderFactoryWrapper())
    {
    }

    /// <summary>
    /// Gets or sets the file path for the Excel file.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// book.FilePath = "C:\files\myfile.xlsx";
    /// ]]>
    /// </example>
    public string FilePath { get; set; } = string.Empty;
    /// <summary>
    /// Gets the loaded data set from the Excel file.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// DataSet ds = book.Data;
    /// ]]>
    /// </example>
    public virtual DataSet Data { get; private set; } = new DataSet();

    /// <summary>
    /// Opens and reads the Excel file from a file path.
    /// </summary>
    /// <param name="filePath">Path to the Excel file.</param>
    /// <returns>A service response indicating success or error.</returns>
    /// <example>
    /// <![CDATA[
    /// var book = new Book();
    /// var result = book.Open("document.xlsx");
    /// ]]>
    /// </example>
    public virtual ServiceResponse Open(string filePath)
    {
        FilePath = filePath;
        try
        {
            using var stream = fileOpener.Open(FilePath);
            return Open(stream);
        }
        catch (Exception ex)
        {
            return ServiceResponse.CreateError(-1, ex.Message);
        }
    }

    /// <summary>
    /// Opens and reads the Excel file from a stream.
    /// </summary>
    /// <param name="stream">Input stream of the Excel file.</param>
    /// <returns>A service response indicating success or error.</returns>
    /// <example>
    /// <![CDATA[
    /// using var stream = File.OpenRead("document.xlsx");
    /// var book = new Book();
    /// var result = book.Open(stream);
    /// ]]>
    /// </example>
    public virtual ServiceResponse Open(Stream stream)
    {
        try
        {
            using var reader = readerFactory.CreateReader(stream, new ExcelReaderConfiguration { FallbackEncoding = Encoding.GetEncoding(1252) });
            Data = reader.AsDataSet();
            if (Data == null || Data.Tables.Count == 0)
                return ServiceResponse.CreateError(5, "Invalid or empty Excel file.");
            return ServiceResponse.CreateSuccess(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return ServiceResponse.CreateError(-1, ex.Message);
        }
    }

    /// <summary>
    /// Returns the number of worksheets in the Excel file.
    /// </summary>
    /// <returns>The number of worksheets.</returns>
    /// <example>
    /// <![CDATA[
    /// int sheetCount = book.GetSheetCount();
    /// ]]>
    /// </example>
    public int GetSheetCount() => Data?.Tables.Count ?? 0;
    /// <summary>
    /// Returns the number of rows in the specified worksheet.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <returns>The number of rows in the worksheet.</returns>
    /// <example>
    /// <![CDATA[
    /// int rows = book.GetRowCount(0);
    /// ]]>
    /// </example>
    public int GetRowCount(int sheetIndex) => Data.Tables[sheetIndex].Rows.Count;
    /// <summary>
    /// Returns the number of columns in the specified worksheet.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <returns>The number of columns in the worksheet.</returns>
    /// <example>
    /// <![CDATA[
    /// int columns = book.GetColumnCount(0);
    /// ]]>
    /// </example>
    public int GetColumnCount(int sheetIndex) => Data.Tables[sheetIndex].Columns.Count;
    /// <summary>
    /// Returns the name of the specified worksheet.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <returns>The name of the worksheet.</returns>
    /// <example>
    /// <![CDATA[
    /// string name = book.GetSheetName(0);
    /// ]]>
    /// </example>
    public string GetSheetName(int sheetIndex) => Data.Tables[sheetIndex].TableName;
    /// <summary>
    /// Reads a cell and converts its value to decimal using invariant culture.
    /// </summary>
    /// <param name="sheetIndex">Index of the sheet.</param>
    /// <param name="columnLetter">Column letter (e.g. 'A').</param>
    /// <param name="rowIndex">1-based row index.</param>
    /// <returns>The decimal value of the cell, or 0 if parsing fails.</returns>
    /// <example>
    /// <![CDATA[
    /// decimal value = book.ReadDecimalCell(0, "A", 1);
    /// ]]>
    /// </example>
    public decimal ReadDecimalCell(int sheetIndex, string columnLetter, int rowIndex)
    {
        var value = ReadCell(sheetIndex, columnLetter, rowIndex);
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : 0m;
    }
    /// <summary>
    /// Reads a cell and converts its value to float using invariant culture.
    /// </summary>
    /// <param name="sheetIndex">Index of the sheet.</param>
    /// <param name="columnLetter">Column letter (e.g. 'B').</param>
    /// <param name="rowIndex">1-based row index.</param>
    /// <returns>The float value of the cell, or 0 if parsing fails.</returns>
    /// <example>
    /// <![CDATA[
    /// float value = book.ReadFloatCell(0, "B", 1);
    /// ]]>
    /// </example>
    public float ReadFloatCell(int sheetIndex, string columnLetter, int rowIndex)
    {
        var value = ReadCell(sheetIndex, columnLetter, rowIndex);
        return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : 0f;
    }
    /// <summary>
    /// Reads a cell and converts its value to int.
    /// </summary>
    /// <param name="sheetIndex">Index of the sheet.</param>
    /// <param name="columnLetter">Column letter (e.g. 'D').</param>
    /// <param name="rowIndex">1-based row index.</param>
    /// <returns>The int value of the cell, or 0 if parsing fails.</returns>
    /// <example>
    /// <![CDATA[
    /// int value = book.ReadInt32Cell(0, "D", 1);
    /// ]]>
    /// </example>
    public int ReadInt32Cell(int sheetIndex, string columnLetter, int rowIndex)
    {
        var value = ReadCell(sheetIndex, columnLetter, rowIndex);
        return int.TryParse(value, out var result)
            ? result
            : 0;
    }
    /// <summary>
    /// Reads a cell and converts its value to long.
    /// </summary>
    /// <param name="sheetIndex">Index of the sheet.</param>
    /// <param name="columnLetter">Column letter (e.g. 'E').</param>
    /// <param name="rowIndex">1-based row index.</param>
    /// <returns>The long value of the cell, or 0 if parsing fails.</returns>
    /// <example>
    /// <![CDATA[
    /// long value = book.ReadInt64Cell(0, "E", 1);
    /// ]]>
    /// </example>
    public long ReadInt64Cell(int sheetIndex, string columnLetter, int rowIndex)
    {
        var value = ReadCell(sheetIndex, columnLetter, rowIndex);
        return long.TryParse(value, out var result)
            ? result
            : 0L;
    }
    /// <summary>
    /// Reads a cell and converts its value to DateTime.
    /// </summary>
    /// <param name="sheetIndex">Index of the sheet.</param>
    /// <param name="columnLetter">Column letter (e.g. 'F').</param>
    /// <param name="rowIndex">1-based row index.</param>
    /// <returns>The DateTime value of the cell, or DateTime.MinValue if parsing fails.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime date = book.ReadDateCell(0, "F", 1);
    /// ]]>
    /// </example>
    public DateTime ReadDateCell(int sheetIndex, string columnLetter, int rowIndex)
    {
        var value = ReadCell(sheetIndex, columnLetter, rowIndex);
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)
            ? result
            : DateTime.MinValue;
    }
    /// <summary>
    /// Reads a string value from a specific cell by column letter.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "G").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>String value or empty string.</returns>
    /// <example>
    /// <![CDATA[
    /// string value = book.ReadCell(0, "G", 7);
    /// ]]>
    /// </example>
    public string ReadCell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        if (_row >= Data.Tables[sheetIndex].Rows.Count || _col >= Data.Tables[sheetIndex].Columns.Count)
        {
            return string.Empty;
        }

        return Data.Tables[sheetIndex].Rows[_row][_col]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Reads a string value from a specific cell by column index.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnIndex">The 1-based index of the column.</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>String value or empty string.</returns>
    /// <example>
    /// <![CDATA[
    /// string value = book.ReadCell(0, 8, 7);
    /// ]]>
    /// </example>
    public string ReadCell(int sheetIndex, int columnIndex, int row)
    {
        var _col = columnIndex - 1;
        var _row = row - 1;
        if (_row >= Data.Tables[sheetIndex].Rows.Count || _col >= Data.Tables[sheetIndex].Columns.Count)
        {
            return string.Empty;
        }

        return Data.Tables[sheetIndex].Rows[_row][_col]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Converts an Excel column letter to a 0-based index.
    /// </summary>
    /// <param name="column">The Excel-style column letter (e.g. "A", "AB").</param>
    /// <returns>Zero-based column index.</returns>
    /// <example>
    /// <![CDATA[
    /// int index = Book.ColumnToIndex("C"); // returns 2
    /// ]]>
    /// </example>
    private static int ColumnToIndex(string column)
    {
        var _bytes = Encoding.ASCII.GetBytes(column.ToUpper());
        var _base = Encoding.ASCII.GetBytes("Z")[0] - Encoding.ASCII.GetBytes("A")[0] + 1;
        var _result = 0;

        for (var i = 0; i < _bytes.Length; i++)
        {
            var _value = _bytes[i] - Encoding.ASCII.GetBytes("A")[0] + 1;
            _result = _result * _base + _value;
        }

        return _result - 1;
    }
}
