using FluentAssertions;
using SOLTEC.Core.Converters;
using SOLTEC.Core.Tests.DTOS;

namespace SOLTEC.Core.Tests.Converters;

public class CsvConverterShould 
{
    private CsvConverter _csvConverter;

    [SetUp]
    public void SetUp() 
    {
        _csvConverter = new CsvConverter();
    }

    [Test]
    public void convert_list_to_csv() 
    {
        var dataList = GetObjetList();
        var expectedRow = GetExpectedCsvRow();
        var i = 0;

        var result = _csvConverter.Transform(dataList, '|');

        result.Count().Should().Be(expectedRow.Count());
        result.ForEach(row => {
            row.Should().Be(expectedRow.ElementAt(i));
            i++;
        });
    }

    private List<string> GetExpectedCsvRow() 
    {
        return new List<string> 
        {
            "Id|Name|Description",
            "1|JhonMe|Test"
        };
    }

    private List<TestDto> GetObjetList() 
    {
        return new List<TestDto> { new() { Id = 1,Name = "Jhon&Me\r\n,", Description = "Test\t'"} };
    }
}
