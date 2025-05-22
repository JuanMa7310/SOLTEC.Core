using SOLTEC.Core.Extensions;
using System.Globalization;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// NUnit tests for <see cref="DateExtensions"/>.
/// </summary>
public class DateExtensionsTests
{
    [Test]
    /// <summary>
    /// Tests formatting null DateTime? returns empty string.
    /// </summary>
    public void ToDateFormat_Null_ReturnsEmptyString()
    {
        DateTime? _input = null;
        Assert.That(_input.ToDateFormat(), Is.EqualTo(string.Empty));
    }

    [Test]
    /// <summary>
    /// Tests nullable date formatting with time.
    /// </summary>
    public void ToDateFormatWithTime_NullableDate_ReturnsFormatted()
    {
        DateTime? _input = new DateTime(2025, 5, 21, 13, 45, 30);
        Assert.That(_input.ToDateFormatWithTime(), Is.EqualTo("20250521134530"));
    }

    [Test]
    /// <summary>
    /// Tests non-nullable date formatting with time.
    /// </summary>
    public void ToDateFormatWithTime_NonNullable_ReturnsFormatted()
    {
        var _input = new DateTime(2025, 5, 21, 13, 45, 30);
        Assert.That(_input.ToDateFormatWithTime(), Is.EqualTo("20250521134530"));
    }

    [Test]
    /// <summary>
    /// Tests ISO8601 formatting.
    /// </summary>
    public void ToDateFormatWithTimeISO8601_ReturnsIso()
    {
        var _input = new DateTime(2025, 5, 21, 13, 45, 30, DateTimeKind.Utc);
        Assert.That(_input.ToDateFormatWithTimeISO8601(), Is.EqualTo("2025-05-21T13:45:30Z"));
    }

    [Test]
    /// <summary>
    /// Tests ParseExactOrDefault with invalid input.
    /// </summary>
    public void ParseExactOrDefault_InvalidFormat_ReturnsDefault()
    {
        var _default = new DateTime(2000, 1, 1);
        Assert.That(DateExtensions.ParseExactOrDefault("invalid", "yyyyMMdd", CultureInfo.InvariantCulture, _default), Is.EqualTo(_default));
    }

    [Test]
    /// <summary>
    /// Tests ParsePart with valid input.
    /// </summary>
    public void ParsePart_ValidInput_ReturnsDate()
    {
        var _result = DateExtensions.ParsePart("X20250521", "\\d{8}", "yyyyMMdd", CultureInfo.InvariantCulture, null);
        Assert.That(_result, Is.EqualTo(new DateTime(2025, 5, 21)));
    }

    [Test]
    /// <summary>
    /// Integration test: format and parse cycle returns original date.
    /// </summary>
    public void FormatParseCycle_ReturnsOriginal()
    {
        var _original = new DateTime(2025, 5, 21, 13, 45, 30, DateTimeKind.Utc);
        var _formatted = _original.ToDateFormatWithTimeISO8601();
        var _parsed = DateExtensions.ParseExactOrDefault(_formatted, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture, null);
        Assert.That(_parsed, Is.EqualTo(_original));
    }
}
