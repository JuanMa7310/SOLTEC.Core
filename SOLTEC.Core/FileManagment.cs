namespace SOLTEC.Core;

using SOLTEC.Core.Enums;
using System.Text;

/// <summary>
/// Provides common file operations such as reading, writing, copying, moving, deleting,
/// and Base64 encoding/decoding of files.
/// </summary>
/// <example>
/// <![CDATA[
/// var _fileManager = new FileManagment();
/// string _name = fileManager.ExtractFileNameFromPath(@"C:\docs\file.txt"); // "file"
/// ]]>
/// </example>
public class FileManagment
{
    /// <summary>
    /// Extracts the file name without its extension from a given file path.
    /// </summary>
    /// <param name="filePath">The full path of the file.</param>
    /// <returns>The file name without extension, or an empty string if the path is invalid.</returns>
    /// <example>
    /// <![CDATA[
    /// var _name = ExtractFileNameFromPath(@"C:\files\report.pdf"); // "report"
    /// ]]>
    /// </example>
    public virtual string ExtractFileNameFromPath(string filePath)
    {
        return string.IsNullOrWhiteSpace(filePath)
            ? string.Empty
            : Path.GetFileNameWithoutExtension(filePath);
    }

    /// <summary>
    /// Extracts the file extension (without the dot) from a given file path.
    /// </summary>
    /// <param name="filePath">The full path of the file.</param>
    /// <returns>The extension without the dot, or an empty string if the path is invalid.</returns>
    /// <example>
    /// <![CDATA[
    /// var _ext = ExtractExtensionFileFromPath(@"file.json"); // "json"
    /// ]]>
    /// </example>
    public virtual string ExtractExtensionFileFromPath(string filePath)
    {
        return string.IsNullOrWhiteSpace(filePath)
            ? string.Empty
            : Path.GetExtension(filePath).TrimStart('.');
    }

    /// <summary>
    /// Creates a text file and writes the given content using UTF-8 encoding.
    /// </summary>
    /// <param name="filePath">The file path to create.</param>
    /// <param name="content">The string content to write.</param>
    /// <example>
    /// <![CDATA[
    /// CreateFile(@"C:\temp\data.txt", "Hello World");
    /// ]]>
    /// </example>
    public virtual void CreateFile(string filePath, string content)
    {
        File.WriteAllText(filePath, content ?? string.Empty, Encoding.UTF8);
    }

    /// <summary>
    /// Creates a file and writes the given binary content.
    /// </summary>
    /// <param name="filePath">The file path to create.</param>
    /// <param name="content">Byte array to write.</param>
    /// <example>
    /// <![CDATA[
    /// CreateFile(@"C:\temp\image.bin", imageBytes);
    /// ]]>
    /// </example>
    public virtual void CreateFile(string filePath, byte[] content)
    {
        File.WriteAllBytes(filePath, content ?? []);
    }

    /// <summary>
    /// Returns all files in a directory that match the specified file type.
    /// </summary>
    /// <param name="directoryPath">Directory to search in.</param>
    /// <param name="fileTypeEnum">File extension to filter (e.g., Json, Xlsx).</param>
    /// <returns>Enumerable of matching file paths, or empty list if path not found.</returns>
    /// <example>
    /// <![CDATA[
    /// var _jsonFiles = GetAllFilesByTypeFromPath(@"C:\configs", FileTypeEnum.Json);
    /// ]]>
    /// </example>
    public virtual IEnumerable<string> GetAllFilesByTypeFromPath(string directoryPath, FileTypeEnum fileTypeEnum)
    {
        return Directory.Exists(directoryPath)
            ? Directory.EnumerateFiles(directoryPath, $"*.{fileTypeEnum.ToString().ToLower()}")
            : [];
    }

    /// <summary>
    /// Copies a file from source to destination, overwriting if exists.
    /// </summary>
    /// <param name="sourcePath">Original file path.</param>
    /// <param name="targetPath">Destination path.</param>
    /// <example>
    /// <![CDATA[
    /// CopyFile(@"C:\old\file.txt", @"D:\new\file.txt");
    /// ]]>
    /// </example>
    public virtual void CopyFile(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, targetPath, overwrite: true);
        }
    }

    /// <summary>
    /// Moves a file from source to destination.
    /// </summary>
    /// <param name="sourcePath">Original file path.</param>
    /// <param name="targetPath">Destination path.</param>
    /// <example>
    /// <![CDATA[
    /// MoveFile(@"C:\docs\temp.txt", @"C:\docs\final.txt");
    /// ]]>
    /// </example>
    public virtual void MoveFile(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Move(sourcePath, targetPath);
        }
    }

    /// <summary>
    /// Deletes a file if it exists.
    /// </summary>
    /// <param name="filePath">Path of the file to delete.</param>
    /// <example>
    /// <![CDATA[
    /// DeleteFile(@"C:\temp\old.log");
    /// ]]>
    /// </example>
    public virtual void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Reads all text from a file asynchronously.
    /// </summary>
    /// <param name="filePath">File path to read from.</param>
    /// <returns>File content or empty string if file doesn't exist.</returns>
    /// <example>
    /// <![CDATA[
    /// string _content = await ReadFileAsync(@"notes.txt");
    /// ]]>
    /// </example>
    public virtual async Task<string> ReadFileAsync(string filePath)
    {
        return File.Exists(filePath)
            ? await File.ReadAllTextAsync(filePath)
            : string.Empty;
    }

    /// <summary>
    /// Writes all lines to a file asynchronously.
    /// </summary>
    /// <param name="filePath">File path to write to.</param>
    /// <param name="rows">Lines of text to write.</param>
    /// <example>
    /// <![CDATA[
    /// await WriteAllLinesAsync("log.txt", new[] { "Start", "End" });
    /// ]]>
    /// </example>
    public virtual async Task WriteAllLinesAsync(string filePath, IEnumerable<string> rows)
    {
        await File.WriteAllLinesAsync(filePath, rows ?? []);
    }

    /// <summary>
    /// Converts a file's binary content to a Base64 string asynchronously.
    /// </summary>
    /// <param name="filePath">File path to encode.</param>
    /// <returns>Base64 string or empty string if file not found.</returns>
    /// <example>
    /// <![CDATA[
    /// string _base64 = await ConvertFileToBase64Async("image.png");
    /// ]]>
    /// </example>
    public virtual async Task<string> ConvertFileToBase64Async(string filePath)
    {
        if (!File.Exists(filePath))
            return string.Empty;
        byte[] _bytes = await File.ReadAllBytesAsync(filePath);

        return Convert.ToBase64String(_bytes);
    }

    /// <summary>
    /// Converts a Base64 string to a memory stream.
    /// </summary>
    /// <param name="base64EncodedData">The Base64 encoded string.</param>
    /// <returns>MemoryStream with decoded content, or empty stream if input is invalid.</returns>
    /// <example>
    /// <![CDATA[
    /// using var _stream = DecodeBase64ToStream(encodedText);
    /// ]]>
    /// </example>
    public virtual Stream DecodeBase64ToStream(string base64EncodedData)
    {
        Stream _result;

        try
        {
            var _bytes = Convert.FromBase64String(base64EncodedData);
            _result = new MemoryStream(_bytes);
        }
        catch (FormatException)
        {
            _result = new MemoryStream(); // empty stream
        }

        return _result;
    }
}