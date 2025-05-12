namespace SOLTEC.Core.Enums;

/// <summary>
/// Specifies the possible reasons why an adapter operation may fail.
/// </summary>
/// <example>
/// <![CDATA[
/// throw new AdapterException(AdapterErrorEnum.ItemsLineNumberError, "Line number mismatch detected.");
/// ]]>
/// </example>
public enum AdapterErrorEnum
{
    /// <summary>
    /// Indicates that the item line number provided is invalid or does not match the expected format.
    /// </summary>
    ItemsLineNumberError
}
