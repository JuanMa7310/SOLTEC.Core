using ExcelDataReader;
using System.Data;
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
public class Book
{
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
    public DataSet Data { get; private set; } = new DataSet();

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
    public ServiceResponse Open(string filePath)
    {
        FilePath = filePath;
        using var _stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
        return Open(_stream);
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
    public ServiceResponse Open(Stream stream)
    {
        try
        {
            using var _reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration
            {
                FallbackEncoding = Encoding.GetEncoding(1252)
            });

            Data = _reader.AsDataSet();

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
    /// Reads a decimal value from a specific cell.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "B").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>Decimal value or null.</returns>
    /// <example>
    /// <![CDATA[
    /// decimal? value = book.ReadDecimalCell(0, "B", 2);
    /// ]]>
    /// </example>
    public decimal? ReadDecimalCell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        var _value = Data.Tables[sheetIndex].Rows[_row][_col]?.ToString();
        return string.IsNullOrWhiteSpace(_value) ? null : Convert.ToDecimal(_value);
    }

    /// <summary>
    /// Reads a float value from a specific cell.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "C").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>Float value or null.</returns>
    /// <example>
    /// <![CDATA[
    /// float? value = book.ReadFloatCell(0, "C", 3);
    /// ]]>
    /// </example>
    public float? ReadFloatCell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        var _value = Data.Tables[sheetIndex].Rows[_row][_col]?.ToString();
        return string.IsNullOrWhiteSpace(_value) ? null : Convert.ToSingle(_value);
    }

    /// <summary>
    /// Reads an integer (Int32) value from a specific cell.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "D").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>Int32 value or null.</returns>
    /// <example>
    /// <![CDATA[
    /// int? value = book.ReadInt32Cell(0, "D", 4);
    /// ]]>
    /// </example>
    public int? ReadInt32Cell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        var _value = Data.Tables[sheetIndex].Rows[_row][_col]?.ToString();
        return string.IsNullOrWhiteSpace(_value) ? null : Convert.ToInt32(_value);
    }

    /// <summary>
    /// Reads a long (Int64) value from a specific cell.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "E").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>Int64 value or null.</returns>
    /// <example>
    /// <![CDATA[
    /// long? value = book.ReadInt64Cell(0, "E", 5);
    /// ]]>
    /// </example>
    public long? ReadInt64Cell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        var _value = Data.Tables[sheetIndex].Rows[_row][_col]?.ToString();
        return string.IsNullOrWhiteSpace(_value) ? null : Convert.ToInt64(_value);
    }

    /// <summary>
    /// Reads a DateTime value from a specific cell.
    /// </summary>
    /// <param name="sheetIndex">The index of the worksheet (0-based).</param>
    /// <param name="columnLetter">Excel-style column letter (e.g., "F").</param>
    /// <param name="row">The row number (1-based).</param>
    /// <returns>DateTime value or null.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime? value = book.ReadDateCell(0, "F", 6);
    /// ]]>
    /// </example>
    public DateTime? ReadDateCell(int sheetIndex, string columnLetter, int row)
    {
        var _col = ColumnToIndex(columnLetter);
        var _row = row - 1;
        try
        {
            var _value = Data.Tables[sheetIndex].Rows[_row][_col]?.ToString();
            if (string.IsNullOrWhiteSpace(_value)) return null;
            return DateTime.TryParse(_value, out var _parsedDate)
                ? _parsedDate
                : DateTime.FromOADate(double.Parse(_value));
        }
        catch { return null; }
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
