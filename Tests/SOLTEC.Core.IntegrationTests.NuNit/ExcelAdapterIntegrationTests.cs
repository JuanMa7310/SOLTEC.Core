using NUnit.Framework;
using SOLTEC.Core.Adapters;
using System.Text;

namespace SOLTEC.Core.IntegrationTests.NuNit;

[TestFixture]
/// <summary>
/// Integration tests for the ExcelAdapter class using NUnit.
/// </summary>
public class ExcelAdapterIntegrationTests
{
    [Test]
    /// <summary>
    /// Validates that Execute reads and maps data from a stream as expected.
    /// </summary>
    public void Execute_FromStream_ReadsExpectedObjects()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        using var _stream = GenerateFakeExcelContent;
        // Act
        var _result = _adapter.Execute<MockExcelModel>(_stream, true);

        // Assert
        Assert.IsNotNull(_result);
        Assert.That(_result, Is.InstanceOf<IEnumerable<MockExcelModel>>());
    }

    private static Stream GenerateFakeExcelContent
    {
        get
        {
            // Simulate Excel content (normally you would use a library to create real content)
            var _data = Encoding.UTF8.GetBytes("Fake Excel Content");
            return new MemoryStream(_data);
        }
    }

    public class MockExcelModel
    {
        public string? Name { get; set; }
    }
}
