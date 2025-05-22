using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for <see cref="IntegerExtensions"/>.
/// </summary>
public class IntegerExtensionsTests
{
    [Fact]
    /// <summary>
    /// Tests parsing valid numeric string.
    /// </summary>
    public void ParseOrDefault_Valid_ReturnsInteger()
    {
        Assert.Equal(123, IntegerExtensions.ParseOrDefault("123", null));
    }

    [Fact]
    /// <summary>
    /// Tests parsing invalid string returns default value.
    /// </summary>
    public void ParseOrDefault_Invalid_ReturnsDefault()
    {
        Assert.Equal(42, IntegerExtensions.ParseOrDefault("abc", 42));
    }

    [Fact]
    /// <summary>
    /// Tests parsing out-of-range numeric string returns default.
    /// </summary>
    public void ParseOrDefault_OutOfRange_ReturnsDefault()
    {
        var _input = int.MaxValue.ToString() + "1";

        Assert.Equal(0, IntegerExtensions.ParseOrDefault(_input, 0));
    }

    [Fact]
    /// <summary>
    /// Integration test: parsing multiple values in a sequence.
    /// </summary>
    public void ParseSequence_Integration_ReturnsExpectedArray()
    {
        var _inputs = new[] { "1", "x", "3" };
        var _expected = new int?[] { 1, -1, 3 };
        var _actual = _inputs.Select(s => IntegerExtensions.ParseOrDefault(s, -1)).ToArray();

        Assert.Equal(_expected, _actual);
    }
}