using SOLTEC.Core.Enums;
using System.Text;

namespace SOLTEC.Core;

public class FileManagment
{
    public virtual string ExtractFileNameFromPath(string filePath)
    {
        return filePath.Split('.')[0];
    }

    public virtual string ExtractExtensionFileFromPath(string filePath)
    {
        return filePath.Split('.')[1];
    }

    public virtual void CreateFile(string filePath, string content)
    {
        using FileStream fs = File.Create(filePath);
        byte[] bytes = new UTF8Encoding(true).GetBytes(content);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }

    public virtual void CreateFile(string filePath, Byte[] content)
    {
        using FileStream fs = File.Create(filePath);
        fs.Write(content, 0, content.Length);
        fs.Close();
    }

    public virtual IEnumerable<string> GetAllFilesByTypeFromPath(string filePath, FileTypeEnum fileTypeEnum)
    {
        return Directory.EnumerateFiles(filePath, $"*.{fileTypeEnum.ToString().ToLower()}");
    }

    public virtual void CopyFile(string sourcePath, string targetPath)
    {
        File.Copy(sourcePath, targetPath);
    }

    public virtual void MoveFile(string sourcePath, string targetPath)
    {
        File.Move(sourcePath, targetPath);
    }

    public virtual void DeleteFile(string filePath)
    {
        if (File.Exists(filePath)) File.Delete(filePath);
    }

    public virtual async Task<string> ReadFileAsync(string filePath) => await File.ReadAllTextAsync(filePath);

    public virtual async Task WriteAllLinesAsync(string filePath, IEnumerable<string> rows) => await File.WriteAllLinesAsync(filePath, rows);

    public virtual async Task<string> ConvertFileToBase64Async(string filePath)
    {
        if (!File.Exists(filePath)) 
            return string.Empty;
        byte[] bytes = await File.ReadAllBytesAsync(filePath);
        return Convert.ToBase64String(bytes);
    }

    public virtual Stream DecodeBaseFile64ToStream(string base64EncodedData)
    {
        var bytes = Convert.FromBase64String(base64EncodedData);

        return new MemoryStream(bytes);
    }
}
