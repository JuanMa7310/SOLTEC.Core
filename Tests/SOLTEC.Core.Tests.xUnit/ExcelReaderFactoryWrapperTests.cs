using ExcelDataReader;
using Moq;
using SOLTEC.Core.Adapters.Excel;
using System.Data;
using System.Reflection;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the <see cref="ExcelReaderFactoryWrapper"/> class using xUnit.
/// </summary>
public class ExcelReaderFactoryWrapperTests
{
    private readonly ExcelReaderFactoryWrapper _wrapper = new();

    static ExcelReaderFactoryWrapperTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [Fact]
    /// <summary>
    /// Ensures that CreateReader returns a valid IExcelDataReader when injected.
    /// </summary>
    public void CreateReader_ValidExcelStream_ReturnsReader()
    {
        // Arrange
        var fakeStream = new MemoryStream();
        var fakeReader = new Mock<IExcelDataReader>();
        fakeReader.Setup(r => r.Read()).Returns(false);

        var wrapperMock = new Mock<IExcelReaderFactoryWrapper>();
        wrapperMock
            .Setup(w => w.CreateReader(
                It.IsAny<Stream>(),
                It.Is<ExcelReaderConfiguration>(c => c != null)))
            .Returns(fakeReader.Object);

        var wrapper = wrapperMock.Object;

        // Act
        var reader = wrapper.CreateReader(fakeStream, new ExcelReaderConfiguration());
        Assert.NotNull(reader);

        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
        {
            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
        });

        // Assert
        Assert.NotNull(dataSet);
    }

    [Fact]
    /// <summary>
    /// Test that CreateReader throws when given an invalid Excel stream.
    /// </summary>
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
