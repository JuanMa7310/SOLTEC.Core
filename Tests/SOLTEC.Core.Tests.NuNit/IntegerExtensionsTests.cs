using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// NUnit tests for <see cref="IntegerExtensions"/>.
/// </summary>
public class IntegerExtensionsTests
{
    [Test]
    /// <summary>
    /// Tests parsing valid numeric string.
    /// </summary>
    public void ParseOrDefault_Valid_ReturnsInteger()
    {
        Assert.That(IntegerExtensions.ParseOrDefault("123", null), Is.EqualTo(123));
    }

    [Test]
    /// <summary>
    /// Tests parsing invalid string returns default value.
    /// </summary>
    public void ParseOrDefault_Invalid_ReturnsDefault()
    {
        Assert.That(IntegerExtensions.ParseOrDefault("abc", 42), Is.EqualTo(42));
    }

    [Test]
    /// <summary>
    /// Tests parsing out-of-range numeric string returns default value.
    /// </summary>
    public void ParseOrDefault_OutOfRange_ReturnsDefault()
    {
        var _input = int.MaxValue.ToString() + "1";

        Assert.That(IntegerExtensions.ParseOrDefault(_input, 0), Is.EqualTo(0));
    }

    [Test]
    /// <summary>
    /// Integration test: parsing multiple values in a sequence.
    /// </summary>
    public void ParseSequence_Integration_ReturnsExpectedArray()
    {
        var _inputs = new[] { "1", "x", "3" };
        var _expected = new int?[] { 1, -1, 3 };
        var _actual = _inputs.Select(s => IntegerExtensions.ParseOrDefault(s, -1)).ToArray();

        Assert.That(_actual, Is.EqualTo(_expected));
    }
}
