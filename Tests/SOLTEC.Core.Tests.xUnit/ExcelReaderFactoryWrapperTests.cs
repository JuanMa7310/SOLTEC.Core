using ExcelDataReader;
using SOLTEC.Core.Adapters.Excel;
using System.Reflection;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the <see cref="ExcelReaderFactoryWrapper"/> class using xUnit.
/// </summary>
public class ExcelReaderFactoryWrapperTests
{
    private readonly ExcelReaderFactoryWrapper _wrapper = new();

    /// <summary>
    /// Test that CreateReader returns a valid <see cref="IExcelDataReader"/> for a known good Excel file.
    /// </summary>
    [Fact]
    public void CreateReader_ValidExcelStream_ReturnsReader()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "SOLTEC.Core.Adapters.Excel.Tests.XUnit.TestData.Sample.xlsx";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        Assert.NotNull(stream);

        // Act
        using var reader = _wrapper.CreateReader(stream!, new ExcelReaderConfiguration { FallbackEncoding = Encoding.UTF8 });

        // Assert
        Assert.NotNull(reader);
        var ds = reader.AsDataSet();
        Assert.NotNull(ds);
        Assert.True(ds.Tables.Count > 0, "DataSet should contain at least one DataTable.");
    }

    /// <summary>
    /// Test that CreateReader throws when given an invalid Excel stream.
    /// </summary>
    [Fact]
    public void CreateReader_InvalidStream_ThrowsException()
    {
        // Arrange
        using var badStream = new MemoryStream([0x00]);

        // Act & Assert
        Assert.ThrowsAny<Exception>(() =>
        {
            using var reader = _wrapper.CreateReader(badStream, new ExcelReaderConfiguration());
            reader.AsDataSet();
        });
    }
}
