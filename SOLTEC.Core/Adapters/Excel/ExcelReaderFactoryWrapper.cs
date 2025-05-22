using ExcelDataReader;

namespace SOLTEC.Core.Adapters.Excel;

/// <summary>
/// Default wrapper around <see cref="ExcelReaderFactory"/> to create Excel readers.
/// </summary>
/// <example>
/// <![CDATA[
/// IExcelReaderFactoryWrapper factory = new ExcelReaderFactoryWrapper();
/// using var reader = factory.CreateReader(stream, new ExcelReaderConfiguration());
/// ]]>
/// </example>
public class ExcelReaderFactoryWrapper : IExcelReaderFactoryWrapper
{
    /// <summary>
    /// Creates an <see cref="IExcelDataReader"/> using <see cref="ExcelReaderFactory.CreateReader"/>.
    /// </summary>
    /// <param name="stream">The input <see cref="Stream"/> containing Excel data.</param>
    /// <param name="configuration">Configuration options for the Excel reader.</param>
    /// <returns>An <see cref="IExcelDataReader"/> instance.</returns>
    public IExcelDataReader CreateReader(Stream stream, ExcelReaderConfiguration configuration)
        => ExcelReaderFactory.CreateReader(stream, configuration);
}
