using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for <see cref="ListExtensions"/>.
/// </summary>
public class ListExtensionsTests
{
    [Fact]
    /// <summary>
    /// Tests IsNullOrEmpty returns true for null list.
    /// </summary>
    public void IsNullOrEmpty_NullList_ReturnsTrue()
    {
        IList<int>? _list = null;

        Assert.True(_list!.IsNullOrEmpty());
    }

    [Fact]
    /// <summary>
    /// Tests IsNullOrEmpty returns true for empty list.
    /// </summary>
    public void IsNullOrEmpty_EmptyList_ReturnsTrue()
    {
        var _list = new List<int>();

        Assert.True(_list.IsNullOrEmpty());
    }

    [Fact]
    /// <summary>
    /// Tests IsNullOrEmpty returns false for non-empty list.
    /// </summary>
    public void IsNullOrEmpty_NonEmptyList_ReturnsFalse()
    {
        var _list = new List<int> { 1 };

        Assert.False(_list.IsNullOrEmpty());
    }

    [Fact]
    /// <summary>
    /// Integration test: clearing a non-empty list makes it empty.
    /// </summary>
    public void ClearList_Integration_MakesListEmpty()
    {
        var _list = new List<int> { 1, 2, 3 };
        _list.Clear();

        Assert.True(_list.IsNullOrEmpty());
    }
}
