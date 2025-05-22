using SOLTEC.Core.Extensions;
using System.Globalization;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// xUnit tests for <see cref="DateExtensions"/>.
/// </summary>
public class DateExtensionsTests
{
    [Fact]
    /// <summary>
    /// Tests formatting null DateTime? returns empty string.
    /// </summary>
    public void ToDateFormat_Null_ReturnsEmptyString()
    {
        DateTime? _input = null;

        Assert.Equal(string.Empty, _input.ToDateFormat());
    }

    [Fact]
    /// <summary>
    /// Tests nullable date formatting with time.
    /// </summary>
    public void ToDateFormatWithTime_NullableDate_ReturnsFormatted()
    {
        DateTime? _input = new DateTime(2025, 5, 21, 13, 45, 30);

        Assert.Equal("20250521134530", _input.ToDateFormatWithTime());
    }

    [Fact]
    /// <summary>
    /// Tests non-nullable date formatting with time.
    /// </summary>
    public void ToDateFormatWithTime_NonNullable_ReturnsFormatted()
    {
        var _input = new DateTime(2025, 5, 21, 13, 45, 30);

        Assert.Equal("20250521134530", _input.ToDateFormatWithTime());
    }

    [Fact]
    /// <summary>
    /// Tests ISO8601 formatting.
    /// </summary>
    public void ToDateFormatWithTimeISO8601_ReturnsIso()
    {
        var _input = new DateTime(2025, 5, 21, 13, 45, 30, DateTimeKind.Utc);

        Assert.Equal("2025-05-21T13:45:30Z", _input.ToDateFormatWithTimeISO8601());
    }

    [Fact]
    /// <summary>
    /// Tests ParseExactOrDefault with invalid input.
    /// </summary>
    public void ParseExactOrDefault_InvalidFormat_ReturnsDefault()
    {
        var _default = new DateTime(2000, 1, 1);

        Assert.Equal(_default, DateExtensions.ParseExactOrDefault("invalid", "yyyyMMdd", CultureInfo.InvariantCulture, _default));
    }

    [Fact]
    /// <summary>
    /// Tests ParsePart with valid input.
    /// </summary>
    public void ParsePart_ValidInput_ReturnsDate()
    {
        var _result = DateExtensions.ParsePart("X20250521", "\\d{8}", "yyyyMMdd", CultureInfo.InvariantCulture, null);

        Assert.Equal(new DateTime(2025, 5, 21), _result);
    }

    [Fact]
    /// <summary>
    /// Integration test: format and parse cycle returns original date.
    /// </summary>
    public void FormatParseCycle_ReturnsOriginal()
    {
        var _original = new DateTime(2025, 5, 21, 13, 45, 30, DateTimeKind.Utc);
        var _formatted = _original.ToDateFormatWithTimeISO8601();
        var _parsed = DateExtensions.ParseExactOrDefault(_formatted, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture, null);

        Assert.Equal(_original, _parsed);
    }
}
