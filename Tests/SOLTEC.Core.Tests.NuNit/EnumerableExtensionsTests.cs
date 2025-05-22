using SOLTEC.Core.Extensions;
using System.Data;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// NUnit tests for <see cref="EnumerableExtensions"/>.
/// </summary>
public class EnumerableExtensionsTests
{
    [Test]
    /// <summary>
    /// Tests ToDataTable throws ArgumentNullException when data is null.
    /// </summary>
    public void ToDataTable_NullData_ThrowsArgumentNullException()
    {
        IEnumerable<string>? _data = null;

        Assert.Throws<ArgumentNullException>(() => _data!.ToDataTable());
    }

    [Test]
    /// <summary>
    /// Tests ToDataTable on empty collection creates columns but no rows.
    /// </summary>
    public void ToDataTable_EmptyCollection_HasColumnsNoRows()
    {
        var _data = new List<TestEntity>();
        var _table = _data.ToDataTable();

        Assert.Multiple(() =>
        {
            Assert.That(_table.Rows, Is.Empty);
            Assert.That(_table.Columns, Has.Count.EqualTo(2));
        });
    }

    [Test]
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

        Assert.That(_table.Rows, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(_table.Rows[0]["Id"], Is.EqualTo(1));
            Assert.That(_table.Rows[0]["Name"], Is.EqualTo("Alice"));
            Assert.That(_table.Rows[1]["Name"], Is.EqualTo(DBNull.Value));
        });
    }

    [Test]
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
            .Select(static r => new TestEntity
            {
                Id = (int)r["Id"],
                Name = r["Name"] as string
            }).ToList();

        Assert.That(_reconstructed, Has.Count.EqualTo(_original.Count));
        Assert.That(_reconstructed[0].Name, Is.EqualTo(_original[0].Name));
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
