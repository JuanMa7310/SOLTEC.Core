using ExcelDataReader;

namespace SOLTEC.Core.Adapters.Excel;

/// <summary>
/// Defines a factory for creating instances of <see cref="IExcelDataReader"/>.
/// </summary>
/// <example>
/// <![CDATA[
/// IExcelReaderFactoryWrapper factory = new ExcelReaderFactoryWrapper();
/// using var reader = factory.CreateReader(stream, new ExcelReaderConfiguration());
/// ]]>
/// </example>
public interface IExcelReaderFactoryWrapper
{
    /// <summary>
    /// Creates an <see cref="IExcelDataReader"/> to read Excel data from the provided stream.
    /// </summary>
    /// <param name="stream">The input <see cref="Stream"/> containing Excel data.</param>
    /// <param name="configuration">Configuration options for the Excel reader.</param>
    /// <returns>An <see cref="IExcelDataReader"/> instance.</returns>
    IExcelDataReader CreateReader(Stream stream, ExcelReaderConfiguration configuration);
}
