namespace SOLTEC.Core.Adapters.Excel;

/// <summary>
/// Provides a default file opening implementation using <see cref="File"/> APIs.
/// </summary>
/// <example>
/// <![CDATA[
/// IFileOpener fileOpener = new DefaultFileOpener();
/// using Stream stream = fileOpener.Open("C:\\data\\report.xlsx");
/// ]]>
/// </example>
public class DefaultFileOpener : IFileOpener
{
    /// <summary>
    /// Opens a file at the specified path with <see cref="FileMode.Open"/> and <see cref="FileAccess.Read"/>.
    /// </summary>
    /// <param name="path">The file system path to open.</param>
    /// <returns>A <see cref="Stream"/> for reading the file content.</returns>
    public Stream Open(string path) => File.Open(path, FileMode.Open, FileAccess.Read);
}
