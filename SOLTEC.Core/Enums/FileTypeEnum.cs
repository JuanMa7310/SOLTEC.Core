namespace SOLTEC.Core.Enums;

/// <summary>
/// Specifies the supported file types used for file operations, such as reading, writing or exporting.
/// </summary>
/// <remarks>
/// This enumeration is typically used to determine the format of a file being handled.
/// </remarks>
/// <example>
/// Example usage:
/// <![CDATA[
/// FileTypeEnum type = FileTypeEnum.Json;
/// if (type == FileTypeEnum.Xlsx)
/// {
///     Console.WriteLine("Exporting to Excel format.");
/// }
/// ]]>
/// </example>
public enum FileTypeEnum
{
    /// <summary>
    /// JavaScript Object Notation file (.json).
    /// Commonly used for configuration or data exchange formats.
    /// </summary>
    /// <example>
    /// FileTypeEnum fileType = FileTypeEnum.Json;
    /// </example>
    Json,
    /// <summary>
    /// Microsoft Excel file (.xlsx).
    /// Often used for spreadsheets and tabular data exports.
    /// </summary>
    /// <example>
    /// FileTypeEnum fileType = FileTypeEnum.Xlsx;
    /// </example>
    Xlsx,
    /// <summary>
    /// Microsoft Word document file (.docx).
    /// Used for generating textual reports or formal documents.
    /// </summary>
    /// <example>
    /// FileTypeEnum fileType = FileTypeEnum.Docx;
    /// </example>
    Docx
}