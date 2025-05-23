namespace SOLTEC.Core.Extensions;

using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// Provides extension methods for parsing numeric strings to <see cref="float"/>.
/// </summary>
/// <remarks>
/// This static class contains methods to convert strings with flexible decimal separators ('.' or ',') to <see cref="float"/>.
/// </remarks>
/// <example>
/// <![CDATA[
/// // Example usage:
/// string s1 = "1,234.56";
/// float f1 = s1.ToFloatFlexible();
/// Console.WriteLine(f1); // Output: 1234.56
/// ]]>
/// </example>
public static partial class StringExtensions
{
    /// <summary>
    /// Converts a numeric string that uses '.' or ',' as decimal separator to a <see cref="float"/>.
    /// </summary>
    /// <param name="input">The numeric string to parse.</param>
    /// <returns>The parsed <see cref="float"/> value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null or whitespace.</exception>
    /// <exception cref="FormatException">Thrown when the input is not a valid numeric string.</exception>
    /// <example>
    /// <![CDATA[
    /// // Parsing a string with comma as decimal separator:
    /// string value1 = "1234,56";
    /// float result1 = value1.ToFloatFlexible();
    /// // result1 == 1234.56f
    ///
    /// // Parsing a string with dot as decimal separator:
    /// string value2 = "1234.56";
    /// float result2 = value2.ToFloatFlexible();
    /// // result2 == 1234.56f
    /// ]]>
    /// </example>
    public static float ToFloatFlexible(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input), "Input string cannot be null or whitespace.");
        }
        input = input.Trim();
        // European format case: thousands point + decimal point (ex: 1,234.56)
        if (EuropeanFormat().IsMatch(input))
        {
            input = input.Replace(".", "").Replace(",", ".");
        }
        // American format case: thousands comma + decimal point (ex: 1,234.56)
        else if (AmericanFormat().IsMatch(input))
        {
            input = input.Replace(",", "");
        }
        // Simple decimal case with comma (ex: 1234.56)
        else if (SimpleDecimalComma().IsMatch(input))
        {
            input = input.Replace(",", ".");
        }
        if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        throw new FormatException($"Input '{input}' could not be converted to float.");
    }

    // -------------------------
    // REGEX HELPERS OPTIMIZED
    // -------------------------

    [GeneratedRegex(@"^\d{1,3}(\.\d{3})*,\d+$")]
    private static partial Regex EuropeanFormat();
    [GeneratedRegex(@"^\d{1,3}(,\d{3})*\.\d+$")]
    private static partial Regex AmericanFormat();
    [GeneratedRegex(@"^\d+,\d+$")]
    private static partial Regex SimpleDecimalComma();
}
