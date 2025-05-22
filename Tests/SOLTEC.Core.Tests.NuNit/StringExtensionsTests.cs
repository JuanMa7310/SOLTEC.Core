using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// NUnit tests for <see cref="StringExtensions"/>.
/// </summary>
public class StringExtensionsTests
{
    [Test]
    /// <summary>
    /// Converts a string with a dot decimal separator to float.
    /// </summary>
    public void ToFloatFlexible_DotDecimal_ReturnsCorrectValue()
    {
        string input = "1234.56";
        float expected = 1234.56f;
        float result = input.ToFloatFlexible();

        Assert.That(result, Is.EqualTo(expected));
    }
    [Test]
    /// <summary>
    /// Converts a string with a comma decimal separator to float.
    /// </summary>
    public void ToFloatFlexible_CommaDecimal_ReturnsCorrectValue()
    {
        string input = "1234,56";
        float expected = 1234.56f;
        float result = input.ToFloatFlexible();

        Assert.That(result, Is.EqualTo(expected));
    }
    [Test]
    /// <summary>
    /// Converts a string with thousand separators to float.
    /// </summary>
    public void ToFloatFlexible_ThousandSeparators_ReturnsCorrectValue()
    {
        string inputUS = "1,234.56";
        string inputES = "1.234,56";
        float expected = 1234.56f;
        float resultUS = inputUS.ToFloatFlexible();
        float resultES = inputES.ToFloatFlexible();

        Assert.Multiple(() =>
        {
            Assert.That(resultUS, Is.EqualTo(expected));
            Assert.That(resultES, Is.EqualTo(expected));
        });
    }
    [Test]
    /// <summary>
    /// Converts a whole number string to float.
    /// </summary>
    public void ToFloatFlexible_WholeNumber_ReturnsCorrectValue()
    {
        string input = "123456";
        float expected = 123456f;
        float result = input.ToFloatFlexible();

        Assert.That(result, Is.EqualTo(expected));
    }
    [Test]
    /// <summary>
    /// Throws ArgumentNullException when input is null or whitespace.
    /// </summary>
    public void ToFloatFlexible_NullOrWhitespace_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StringExtensions.ToFloatFlexible(null!));
        Assert.Throws<ArgumentNullException>(() => string.Empty.ToFloatFlexible());
        Assert.Throws<ArgumentNullException>(() => "   ".ToFloatFlexible());
    }
    [Test]
    /// <summary>
    /// Throws FormatException when input format is invalid.
    /// </summary>
    public void ToFloatFlexible_InvalidFormat_ThrowsFormatException()
    {
        string input = "abc123";

        Assert.Throws<FormatException>(() => input.ToFloatFlexible());
    }
}
