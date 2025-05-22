using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for <see cref="ArrayExtensions"/>.
/// </summary>
public class ArrayExtensionsTests
{
    [Fact]
    /// <summary>
    /// Tests that Add throws ArgumentNullException when array is null.
    /// </summary>
    public void Add_NullArray_ThrowsArgumentNullException()
    {
        string[]? _input = null;

        Assert.Throws<ArgumentNullException>(() => _input!.Add("item"));
    }

    [Fact]
    /// <summary>
    /// Tests adding an item to the end of the array.
    /// </summary>
    public void Add_End_ReturnsAppendedItem()
    {
        var _original = new[] { "a", "b" };
        var _result = _original.Add("c");

        Assert.Equal(["a", "b", "c"], _result);
    }

    [Fact]
    /// <summary>
    /// Tests adding an item to the beginning of the array.
    /// </summary>
    public void Add_Prepend_ReturnsPrependedItem()
    {
        var _original = new[] { 1, 2 };
        var _result = _original.Add(0, prepend: true);

        Assert.Equal([0, 1, 2], _result);
    }

    [Fact]
    /// <summary>
    /// Integration test: chaining multiple Add calls.
    /// </summary>
    public void AddChain_MultipleCalls_ReturnsCombinedArray()
    {
        var _original = new[] { 1, 2 };
        var _result = _original.Add(3).Add(0, prepend: true);

        Assert.True(_result.SequenceEqual([0, 1, 2, 3]));
    }
}
