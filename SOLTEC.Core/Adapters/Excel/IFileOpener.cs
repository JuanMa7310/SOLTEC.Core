namespace SOLTEC.Core.Adapters.Excel;

/// <summary>
/// Defines a contract for opening files, allowing streams to be provided from various sources.
/// </summary>
/// <example>
/// <![CDATA[
/// IFileOpener fileOpener = new DefaultFileOpener();
/// using Stream stream = fileOpener.Open("path/to/file.xlsx");
/// ]]>
/// </example>
public interface IFileOpener
{
    /// <summary>
    /// Opens a file at the specified path and returns a read-only stream.
    /// </summary>
    /// <param name="path">The file system path to open.</param>
    /// <returns>A <see cref="Stream"/> for reading the file content.</returns>
    Stream Open(string path);
}
