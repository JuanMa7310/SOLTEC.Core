namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides methods to parse strings into integers with a default fallback.
/// </summary>
/// <example>
/// <![CDATA[
/// // Parsing a valid numeric string
/// int? _parsed = IntegerExtensions.ParseOrDefault("123", null); // 123
/// // Parsing an invalid string returns the provided default
/// int? _defaulted = IntegerExtensions.ParseOrDefault("abc", 42); // 42
/// // Null input also returns default
/// int? _nullDefault = IntegerExtensions.ParseOrDefault(null, 0); // 0
/// ]]>
/// </example>
public static class IntegerExtensions
{
    /// <summary>
    /// Parses the specified string to an integer, or returns the given default if parsing fails.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="defaultValue">The value to return if parsing fails.</param>
    /// <returns>
    /// A nullable <see cref="Int32"/> parsed from <paramref name="s"/>, or <paramref name="defaultValue"/> if parsing fails.
    /// </returns>
    /// <example>
    /// <![CDATA[
    /// int? _result1 = IntegerExtensions.ParseOrDefault("100", null);   // 100
    /// int? _result2 = IntegerExtensions.ParseOrDefault("xyz", -1);     // -1
    /// ]]>
    /// </example>
    public static int? ParseOrDefault(string s, int? defaultValue)
    {
        try
        {
            int _parsed = int.Parse(s);
            return _parsed;
        }
        catch
        {
            return defaultValue;
        }
    }
}
