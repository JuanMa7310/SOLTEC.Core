using Ganss.Excel;
using NPOI.OpenXml4Net.Exceptions;

namespace SOLTEC.Core.Adapters;

/// <summary>
/// Adapter that reads Excel files and maps rows to strongly typed objects using ExcelMapper.
/// </summary>
/// <example>
/// <![CDATA[
/// var adapter = new ExcelAdapter();
/// var items = adapter.Execute<MyModel>("file.xlsx", true);
/// ]]>
/// </example>
public class ExcelAdapter : Adapter
{
    /// <summary>
    /// Reads data from an Excel file and maps rows to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type to map rows to.</typeparam>
    /// <param name="pathFile">The full path to the Excel file.</param>
    /// <param name="headerRow">Indicates whether the first row is a header. Default is false.</param>
    /// <param name="sheetName">The optional name of the sheet to read from.</param>
    /// <returns>An enumerable of mapped objects.</returns>
    /// <example>
    /// <![CDATA[
    /// var result = new ExcelAdapter().Execute<MyModel>("data.xlsx", true, "Sheet1");
    /// ]]>
    /// </example>
    public virtual IEnumerable<T> Execute<T>(string pathFile, bool headerRow = false, string? sheetName = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sheetName))
                return new ExcelMapper(pathFile) { HeaderRow = headerRow }.Fetch<T>();
            return new ExcelMapper() { HeaderRow = headerRow }.Fetch<T>(pathFile, sheetName);
        }
        catch(FileNotFoundException) 
        {
            return [];
        }
        catch (InvalidFormatException)
        {
            return [];
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Reads data from a stream and maps rows to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type to map rows to.</typeparam>
    /// <param name="stream">The stream containing the Excel file.</param>
    /// <param name="headerRow">Indicates whether the first row is a header. Default is false.</param>
    /// <param name="sheetName">The optional name of the sheet to read from.</param>
    /// <returns>An enumerable of mapped objects.</returns>
    /// <example>
    /// <![CDATA[
    /// using var stream = File.OpenRead("data.xlsx");
    /// var result = new ExcelAdapter().Execute<MyModel>(stream, true);
    /// ]]>
    /// </example>
    public virtual IEnumerable<T> Execute<T>(Stream stream, bool headerRow = false, string? sheetName = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sheetName))
                return new ExcelMapper(stream) { HeaderRow = headerRow }.Fetch<T>();
            return new ExcelMapper() { HeaderRow = headerRow }.Fetch<T>(stream, sheetName);
        }
        catch (InvalidFormatException)
        {
            return [];
        }
        catch
        {
            return [];
        }
    }
}
