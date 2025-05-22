using System.Text.RegularExpressions;

namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides extension methods for formatting and parsing <see cref="DateTime"/> values.
/// </summary>
/// <example>
/// <![CDATA[
/// // Formatting examples
/// DateTime? nullableDate = new DateTime(2025, 5, 21, 13, 45, 30);
/// string dateOnly = nullableDate.ToDateFormat();            // "20250521"
/// string dateTime = nullableDate.ToDateFormatWithTime();     // "20250521134530"
/// DateTime date = DateTime.UtcNow;
/// string iso8601 = date.ToDateFormatWithTimeISO8601();        // "2025-05-21T13:45:30Z"
///
/// // Parsing examples
/// string input = "OrderDate:20250521";
/// DateTime? parsed = DateExtensions.ParsePart(input, @"\d{8}", "yyyyMMdd", CultureInfo.InvariantCulture, null);
/// ]]>
/// </example>
public static class DateExtensions
{
    /// <summary>
    /// Formats a nullable <see cref="DateTime"/> as "yyyyMMdd". Returns empty string if <paramref name="date"/> is null.
    /// </summary>
    /// <param name="date">The nullable <see cref="DateTime"/> to format.</param>
    /// <returns>A string in "yyyyMMdd" format, or empty if <paramref name="date"/> is null.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime? date = new DateTime(2025, 5, 21);
    /// string result = date.ToDateFormat(); // "20250521"
    /// ]]>
    /// </example>
    public static string ToDateFormat(this DateTime? date)
    {
        return date?.ToString("yyyyMMdd") ?? string.Empty;
    }

    /// <summary>
    /// Formats a nullable <see cref="DateTime"/> as "yyyyMMddHHmmss". Returns empty string if <paramref name="date"/> is null.
    /// </summary>
    /// <param name="date">The nullable <see cref="DateTime"/> to format.</param>
    /// <returns>A string in "yyyyMMddHHmmss" format, or empty if <paramref name="date"/> is null.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime? date = new DateTime(2025, 5, 21, 13, 45, 30);
    /// string result = date.ToDateFormatWithTime(); // "20250521134530"
    /// ]]>
    /// </example>
    public static string ToDateFormatWithTime(this DateTime? date)
    {
        return date?.ToString("yyyyMMddHHmmss") ?? string.Empty;
    }

    /// <summary>
    /// Formats a <see cref="DateTime"/> as "yyyyMMddHHmmss".
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to format.</param>
    /// <returns>A string in "yyyyMMddHHmmss" format.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime date = new DateTime(2025, 5, 21, 13, 45, 30);
    /// string result = date.ToDateFormatWithTime(); // "20250521134530"
    /// ]]>
    /// </example>
    public static string ToDateFormatWithTime(this DateTime date)
    {
        return date.ToString("yyyyMMddHHmmss");
    }

    /// <summary>
    /// Formats a <see cref="DateTime"/> as ISO 8601 "yyyy-MM-ddTHH:mm:ssZ".
    /// </summary>
    /// <param name="date">The <see cref="DateTime"/> to format.</param>
    /// <returns>A string in ISO 8601 format.</returns>
    /// <example>
    /// <![CDATA[
    /// DateTime date = new DateTime(2025, 5, 21, 13, 45, 30);
    /// string iso = date.ToDateFormatWithTimeISO8601(); // "2025-05-21T13:45:30Z"
    /// ]]>
    /// </example>
    public static string ToDateFormatWithTimeISO8601(this DateTime date)
    {
        return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
    }

    /// <summary>
    /// Parses a substring of <paramref name="s"/> matching the specified regex and format.
    /// Returns <paramref name="defaultValue"/> if parsing fails or no match is found.
    /// </summary>
    /// <param name="s">The input string to search.</param>
    /// <param name="regex">The regular expression pattern to match.</param>
    /// <param name="format">The expected date format of the matched substring.</param>
    /// <param name="provider">The format provider for parsing.</param>
    /// <param name="defaultValue">The value to return if parsing fails.</param>
    /// <returns>A nullable <see cref="DateTime"/> parsed from the input, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <![CDATA[
    /// string input = "Date:20250521";
    /// DateTime? date = DateExtensions.ParsePart(input, "\\d{8}", "yyyyMMdd", CultureInfo.InvariantCulture, null);
    /// ]]>
    /// </example>
    public static DateTime? ParsePart(string s, string regex, string format, IFormatProvider provider, DateTime? defaultValue)
    {
        try
        {
            var _match = Regex.Match(s, regex);
            if (_match.Success)
            {
                return ParseExactOrDefault(_match.Groups[0].ToString(), format, provider, defaultValue);
            }

            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Parses <paramref name="s"/> exactly with the given format and provider.
    /// Returns <paramref name="defaultValue"/> if parsing fails.
    /// </summary>
    /// <param name="s">The date string to parse.</param>
    /// <param name="format">The expected date format.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="defaultValue">The value to return if parsing fails.</param>
    /// <returns>A nullable <see cref="DateTime"/> parsed from <paramref name="s"/>, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <![CDATA[
    /// string s = "20250521134530";
    /// DateTime? date = DateExtensions.ParseExactOrDefault(s, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, null);
    /// ]]>
    /// </example>
    public static DateTime? ParseExactOrDefault(string s, string format, IFormatProvider provider, DateTime? defaultValue)
    {
        try
        {
            return DateTime.ParseExact(s, format, provider);
        }
        catch
        {
            return defaultValue;
        }
    }
}
