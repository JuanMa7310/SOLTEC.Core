using SOLTEC.Core.Adapters;
using System.Text;

namespace SOLTEC.Core.Tests.NuNit;


[TestFixture]
/// <summary>
/// NUnit tests for the ExcelAdapter class.
/// </summary>
public class ExcelAdapterTests
{
    /// <summary>
    /// Tests that Execute returns a non-null collection when reading from a stream without a sheet name.
    /// </summary>
    [Test]
    public void Execute_StreamWithoutSheetName_ReturnsData()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        var _fakeContent = CreateFakeExcelStream;
        // Act
        var _result = _adapter.Execute<MockData>(_fakeContent, true);

        // Assert
        Assert.That(_result, Is.Not.Null);
    }

    /// <summary>
    /// Tests that Execute returns a non-null collection when reading from a file with a sheet name.
    /// </summary>
    [Test]
    public void Execute_FileWithSheetName_ReturnsData()
    {
        // Arrange
        var _adapter = new ExcelAdapter();
        var _path = "fake.xlsx";
        // Act
        var _result = _adapter.Execute<MockData>(_path, true, "Sheet1");

        // Assert
        Assert.That(_result, Is.Not.Null);
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
