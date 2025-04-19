using SOLTEC.Core.Enums;
using System.Text;

namespace SOLTEC.Core;

/// <summary>
/// Provides common file operations such as reading, writing, copying, moving, deleting,
/// and base64 encoding/decoding of files.
/// </summary>
public class FileManagement
{
    /// <summary>
    /// Extracts the file name without its extension from a given file path.
    /// </summary>
    /// <param name="filePath">The full path of the file.</param>
    /// <returns>The file name without extension. Returns an empty string if the path is invalid.</returns>
    public virtual string ExtractFileNameFromPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return string.Empty;
        return Path.GetFileNameWithoutExtension(filePath);
    }

    /// <summary>
    /// Extracts the extension of a file (without the dot) from a given file path.
    /// </summary>
    /// <param name="filePath">The full path of the file.</param>
    /// <returns>The file extension without the dot. Returns an empty string if the path is invalid.</returns>
    public virtual string ExtractExtensionFileFromPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return string.Empty;
        return Path.GetExtension(filePath).TrimStart('.');
    }

    /// <summary>
    /// Creates a new file and writes the specified text content encoded in UTF-8.
    /// </summary>
    /// <param name="filePath">The path where the file will be created.</param>
    /// <param name="content">The text content to write into the file.</param>
    public virtual void CreateFile(string filePath, string content)
    {
        File.WriteAllText(filePath, content, Encoding.UTF8);
    }

    /// <summary>
    /// Creates a new file and writes the specified byte array as binary content.
    /// </summary>
    /// <param name="filePath">The path where the file will be created.</param>
    /// <param name="content">The byte array to write into the file.</param>
    public virtual void CreateFile(string filePath, byte[] content)
    {
        File.WriteAllBytes(filePath, content);
    }

    /// <summary>
    /// Retrieves all files with the specified extension from the given directory.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to search.</param>
    /// <param name="fileTypeEnum">The file type (extension) to filter by.</param>
    /// <returns>An enumerable of file paths matching the given extension. Returns an empty list if the directory does not exist.</returns>
    public virtual IEnumerable<string> GetAllFilesByTypeFromPath(string directoryPath, FileTypeEnum fileTypeEnum)
    {
        if (!Directory.Exists(directoryPath)) return new List<string>();
        return Directory.EnumerateFiles(directoryPath, $"*.{fileTypeEnum.ToString().ToLower()}");
    }

    /// <summary>
    /// Copies a file from the source path to the target path, overwriting if the target already exists.
    /// </summary>
    /// <param name="sourcePath">The full path of the source file.</param>
    /// <param name="targetPath">The full path where the file will be copied.</param>
    public virtual void CopyFile(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, targetPath, overwrite: true);
        }
    }

    /// <summary>
    /// Moves a file from the source path to the target path.
    /// </summary>
    /// <param name="sourcePath">The full path of the source file.</param>
    /// <param name="targetPath">The full path where the file will be moved.</param>
    public virtual void MoveFile(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Move(sourcePath, targetPath);
        }
    }

    /// <summary>
    /// Deletes the specified file if it exists.
    /// </summary>
    /// <param name="filePath">The full path of the file to delete.</param>
    public virtual void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Asynchronously reads the entire content of a file as text.
    /// </summary>
    /// <param name="filePath">The full path of the file to read.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the file content as a string, or an empty string if the file does not exist.</returns>
    public virtual async Task<string> ReadFileAsync(string filePath)
    {
        if (!File.Exists(filePath)) return string.Empty;
        return await File.ReadAllTextAsync(filePath);
    }

    /// <summary>
    /// Asynchronously writes all lines of text to a file.
    /// </summary>
    /// <param name="filePath">The full path of the file to write.</param>
    /// <param name="rows">The lines of text to write to the file.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public virtual async Task WriteAllLinesAsync(string filePath, IEnumerable<string> rows)
    {
        await File.WriteAllLinesAsync(filePath, rows);
    }

    /// <summary>
    /// Asynchronously converts a file's content into a Base64-encoded string.
    /// </summary>
    /// <param name="filePath">The full path of the file to convert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Base64-encoded content, or an empty string if the file does not exist.</returns>
    public virtual async Task<string> ConvertFileToBase64Async(string filePath)
    {
        if (!File.Exists(filePath)) return string.Empty;
        byte[] bytes = await File.ReadAllBytesAsync(filePath);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decodes a Base64 string into a memory stream.
    /// </summary>
    /// <param name="base64EncodedData">The Base64 string to decode.</param>
    /// <returns>A memory stream containing the decoded byte array. Returns an empty stream if the input is null or empty.</returns>
    public virtual Stream DecodeBase64ToStream(string base64EncodedData)
    {
        if (string.IsNullOrWhiteSpace(base64EncodedData)) return Stream.Null;
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return new MemoryStream(bytes);
    }
}