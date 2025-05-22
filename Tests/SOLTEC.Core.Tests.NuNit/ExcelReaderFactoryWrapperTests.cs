using ExcelDataReader;
using SOLTEC.Core.Adapters.Excel;
using System.Reflection;
using System.Text;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the <see cref="ExcelReaderFactoryWrapper"/> class.
/// </summary>
public class ExcelReaderFactoryWrapperTests
{
    private ExcelReaderFactoryWrapper _wrapper;

    [SetUp]
    public void Setup()
    {
        _wrapper = new ExcelReaderFactoryWrapper();
    }

    [Test]
    /// <summary>
    /// Test that CreateReader returns a valid <see cref="IExcelDataReader"/> for a known good Excel file.
    /// </summary>
    public void CreateReader_ValidExcelStream_ReturnsReader()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "SOLTEC.Core.Adapters.Excel.Tests.NUnit.TestData.Sample.xlsx";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        Assert.That(stream, Is.Not.Null, $"Embedded resource '{resourceName}' not found.");

        using var reader = _wrapper.CreateReader(stream, new ExcelReaderConfiguration { FallbackEncoding = Encoding.UTF8 });
        Assert.That(reader, Is.Not.Null);

        var ds = reader.AsDataSet();
        Assert.That(ds, Is.Not.Null);
        Assert.That(ds.Tables, Is.Not.Empty, "DataSet should contain at least one DataTable.");
    }

    [Test]
    /// <summary>
    /// Test that CreateReader throws when given an invalid Excel stream.
    /// </summary>
    public void CreateReader_InvalidStream_ThrowsException()
    {
        using var badStream = new MemoryStream([0x00]);
        Assert.Throws<Exception>(() =>
        {
            using var reader = _wrapper.CreateReader(badStream, new ExcelReaderConfiguration());
            reader.AsDataSet();
        });
    }
}
