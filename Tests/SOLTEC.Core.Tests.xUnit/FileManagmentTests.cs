using SOLTEC.Core.Enums;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

public class FileManagmentTests : IDisposable
{
    private readonly FileManagement _fileManager;
    private readonly string _testDir;

    public FileManagmentTests()
    {
        _fileManager = new FileManagement();
        _testDir = Path.Combine(Path.GetTempPath(), "FileManagementTests");
        Directory.CreateDirectory(_testDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    private string GetTempFilePath(string name = "test.txt") => Path.Combine(_testDir, name);

    [Fact]
    public void ExtractFileNameFromPath_ShouldReturnCorrectName()
    {
        string path = @"C:\folder\example.txt";
        string result = _fileManager.ExtractFileNameFromPath(path);

        Assert.Equal("example", result);
    }

    [Fact]
    public void ExtractExtensionFileFromPath_ShouldReturnCorrectExtension()
    {
        string path = @"C:\folder\example.json";
        string result = _fileManager.ExtractExtensionFileFromPath(path);

        Assert.Equal("json", result);
    }

    [Fact]
    public void CreateFile_WithText_ShouldCreateFileAndWriteContent()
    {
        string path = GetTempFilePath();
        string content = "Hello xUnit!";
        _fileManager.CreateFile(path, content);

        Assert.True(File.Exists(path));
        Assert.Equal(content, File.ReadAllText(path));
    }

    [Fact]
    public void CreateFile_WithBytes_ShouldCreateBinaryFile()
    {
        string path = GetTempFilePath("binary.dat");
        byte[] data = Encoding.UTF8.GetBytes("Binary data");
        _fileManager.CreateFile(path, data);

        Assert.True(File.Exists(path));
    }

    [Fact]
    public void GetAllFilesByTypeFromPath_ShouldReturnJsonFiles()
    {
        string filePath = GetTempFilePath("data.json");
        File.WriteAllText(filePath, "{}");
        var files = _fileManager.GetAllFilesByTypeFromPath(_testDir, FileTypeEnum.Json);

        Assert.Contains(filePath, files);
    }

    [Fact]
    public void CopyFile_ShouldDuplicateFile()
    {
        string source = GetTempFilePath("source.txt");
        string target = GetTempFilePath("copied.txt");
        File.WriteAllText(source, "Copy me");
        _fileManager.CopyFile(source, target);

        Assert.True(File.Exists(target));
    }

    [Fact]
    public void MoveFile_ShouldRelocateFile()
    {
        string source = GetTempFilePath("to_move.txt");
        string target = GetTempFilePath("moved.txt");
        File.WriteAllText(source, "Move me");
        _fileManager.MoveFile(source, target);

        Assert.False(File.Exists(source));
        Assert.True(File.Exists(target));
    }

    [Fact]
    public void DeleteFile_ShouldRemoveFile()
    {
        string path = GetTempFilePath("to_delete.txt");
        File.WriteAllText(path, "Remove me");
        _fileManager.DeleteFile(path);

        Assert.False(File.Exists(path));
    }

    [Fact]
    public async Task ReadFileAsync_ShouldReadTextContent()
    {
        string path = GetTempFilePath("read.txt");
        string content = "Read me async";
        await File.WriteAllTextAsync(path, content);
        string result = await _fileManager.ReadFileAsync(path);

        Assert.Equal(content, result);
    }

    [Fact]
    public async Task WriteAllLinesAsync_ShouldWriteMultipleLines()
    {
        string path = GetTempFilePath("lines.txt");
        var lines = new[] { "Line 1", "Line 2", "Line 3" };
        await _fileManager.WriteAllLinesAsync(path, lines);
        var readLines = await File.ReadAllLinesAsync(path);

        Assert.Equal(lines, readLines);
    }

    [Fact]
    public async Task ConvertFileToBase64Async_ShouldEncodeContent()
    {
        string path = GetTempFilePath("base64.txt");
        string content = "Encode me";
        await File.WriteAllTextAsync(path, content);
        string base64 = await _fileManager.ConvertFileToBase64Async(path);
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        Assert.Equal(content, decoded);
    }

    [Fact]
    public void DecodeBase64ToStream_ShouldReturnOriginalStream()
    {
        string original = "Stream this";
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
        using var stream = _fileManager.DecodeBase64ToStream(base64);
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        Assert.Equal(original, result);
    }

    [Fact]
    public async Task ReadFileAsync_NonExistent_ShouldReturnEmpty()
    {
        string path = GetTempFilePath("notfound.txt");
        string result = await _fileManager.ReadFileAsync(path);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void DecodeBase64ToStream_InvalidInput_ShouldReturnEmptyStream()
    {
        var stream = _fileManager.DecodeBase64ToStream("???");

        Assert.Equal(0, stream.Length);
    }
}