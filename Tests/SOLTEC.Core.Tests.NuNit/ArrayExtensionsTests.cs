using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// NUnit tests for <see cref="ArrayExtensions"/>.
/// </summary>
public class ArrayExtensionsTests
{
    [Test]
    /// <summary>
    /// Tests that Add throws ArgumentNullException when array is null.
    /// </summary>
    public void Add_NullArray_ThrowsArgumentNullException()
    {
        string[]? _input = null;

        Assert.Throws<ArgumentNullException>(() => ArrayExtensions.Add(_input!, "item"));
    }
    private static readonly string[] expected = ["a", "b", "c"];
    private static readonly int[] expectedArray = [0, 1, 2];

    [Test]
    /// <summary>
    /// Tests adding an item to the end of the array.
    /// </summary>
    public void Add_End_ReturnsAppendedItem()
    {
        var _original = new[] { "a", "b" };
        var _result = _original.Add("c");

        Assert.That(_result, Is.EqualTo(expected));
    }

    [Test]
    /// <summary>
    /// Tests adding an item to the beginning of the array.
    /// </summary>
    public void Add_Prepend_ReturnsPrependedItem()
    {
        var _original = new[] { 1, 2 };
        var _result = _original.Add(0, prepend: true);

        Assert.That(_result, Is.EqualTo(expectedArray));
    }

    [Test]
    /// <summary>
    /// Integration test: chaining multiple Add calls.
    /// </summary>
    public void AddChain_MultipleCalls_ReturnsCombinedArray()
    {
        var _original = new[] { 1, 2 };
        var _result = _original.Add(3).Add(0, prepend: true);

        Assert.That(_result.SequenceEqual([0, 1, 2, 3]), Is.True);
    }
}