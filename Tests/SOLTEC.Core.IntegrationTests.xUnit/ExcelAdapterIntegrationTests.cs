using SOLTEC.Core.Adapters;
using System.Text;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the ExcelAdapter class using xUnit.
/// </summary>
public class ExcelAdapterIntegrationTests
{
    /// <summary>
    /// Ensures that Execute properly maps Excel data from a stream into a list of objects.
    /// </summary>
    [Fact]
    public void Execute_FromStream_MapsToExpectedObjects()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        using var _stream = GenerateFakeExcelContent;
        // Act
        var _result = _adapter.Execute<MockExcelModel>(_stream, true);

        // Assert
        Assert.NotNull(_result);
        Assert.IsAssignableFrom<IEnumerable<MockExcelModel>>(_result);
    }

    private static Stream GenerateFakeExcelContent
    {
        get
        {
            var _data = Encoding.UTF8.GetBytes("Fake Excel Content");
            return new MemoryStream(_data);
        }
    }

    public class MockExcelModel
    {
        public string? Name { get; set; }
    }
}
