using SOLTEC.Core.Adapters;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for the ExcelAdapter class.
/// </summary>
public class ExcelAdapterTests
{
    /// <summary>
    /// Verifies that Execute with a stream and no sheet name returns a non-null result.
    /// </summary>
    [Fact]
    public void Execute_StreamWithoutSheetName_ReturnsData()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        using var _stream = CreateFakeExcelStream;
        // Act
        var _result = _adapter.Execute<MockData>(_stream, true);

        // Assert
        Assert.NotNull(_result);
    }

    /// <summary>
    /// Verifies that Execute with a file path and sheet name returns a non-null result.
    /// </summary>
    [Fact]
    public void Execute_FileWithSheetName_ReturnsData()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        var _path = "dummy.xlsx";
        // Act
        var _result = _adapter.Execute<MockData>(_path, true, "Sheet1");

        // Assert
        Assert.NotNull(_result);
    }

    private static Stream CreateFakeExcelStream
    {
        get
        {
            var _dummy = new MemoryStream(Encoding.UTF8.GetBytes("fake data"));
            return _dummy;
        }
    }

    public class MockData
    {
        public string? Name { get; set; }
    }
}
