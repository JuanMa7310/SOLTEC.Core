using System.Globalization;

namespace SOLTEC.Core.Extensions;

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
public static  class StringExtensions
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
            throw new ArgumentNullException(nameof(input), "Input string cannot be null or whitespace.");

        // Try parsing with dot as decimal separator (InvariantCulture)
        if (float.TryParse(
                input,
                NumberStyles.Float | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture,
                out var result))
        {
            return result;
        }

        // Try parsing with comma as decimal separator (es-ES style)
        var nfiComma = new NumberFormatInfo
        {
            NumberDecimalSeparator = ",",
            NumberGroupSeparator = "."
        };
        if (float.TryParse(
                input,
                NumberStyles.Float | NumberStyles.AllowThousands,
                nfiComma,
                out result))
        {
            return result;
        }

        throw new FormatException($"'{input}' is not a valid number format.");
    }
}
