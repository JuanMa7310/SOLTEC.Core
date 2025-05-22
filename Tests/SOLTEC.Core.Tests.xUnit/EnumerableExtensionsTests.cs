using SOLTEC.Core.Extensions;
using System.Data;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for <see cref="EnumerableExtensions"/>.
/// </summary>
public class EnumerableExtensionsTests
{
    [Fact]
    /// <summary>
    /// Tests ToDataTable throws ArgumentNullException when data is null.
    /// </summary>
    public void ToDataTable_NullData_ThrowsArgumentNullException()
    {
        IEnumerable<string>? _data = null;

        Assert.Throws<ArgumentNullException>(() => _data!.ToDataTable());
    }

    [Fact]
    /// <summary>
    /// Tests ToDataTable on empty collection creates columns but no rows.
    /// </summary>
    public void ToDataTable_EmptyCollection_HasColumnsNoRows()
    {
        var _data = new List<TestEntity>();
        var _table = _data.ToDataTable();

        Assert.Empty(_table.Rows);
        Assert.Equal(2, _table.Columns.Count);
    }

    [Fact]
    /// <summary>
    /// Tests ToDataTable on populated collection.
    /// </summary>
    public void ToDataTable_PopulatedCollection_ReturnsCorrectRows()
    {
        var _data = new List<TestEntity>
            {
                new() { Id = 1, Name = "Alice" },
                new() { Id = 2, Name = null }
            };
        var _table = _data.ToDataTable();

        Assert.Equal(2, _table.Rows.Count);
        Assert.Equal(1, _table.Rows[0]["Id"]);
        Assert.Equal("Alice", _table.Rows[0]["Name"]);
        Assert.Equal(DBNull.Value, _table.Rows[1]["Name"]);
    }

    [Fact]
    /// <summary>
    /// Integration test: round-trip conversion to DataTable and back to list.
    /// </summary>
    public void RoundTripConversion_Integration_ReturnsOriginalList()
    {
        var _original = new List<TestEntity>
            {
                new() { Id = 3, Name = "Bob" },
                new() { Id = 4, Name = "Carol" }
            };
        var _table = _original.ToDataTable();
        var _reconstructed = _table.Rows.Cast<DataRow>()
            .Select(r => new TestEntity
            {
                Id = (int)r["Id"],
                Name = r["Name"] as string
            }).ToList();

        Assert.Equal(_original.Count, _reconstructed.Count);
        Assert.Equal(_original[0].Name, _reconstructed[0].Name);
    }

    /// <summary>
    /// Helper class for testing.
    /// </summary>
    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
