using SOLTEC.Core.Enums;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the <see cref="FileManagment"/> class using xUnit.
/// Validates behavior for file creation, reading, deletion, and conversion.
/// </summary>
public class FileManagmentTests : IDisposable
{
    private readonly FileManagment _fileManager;
    private readonly string _testDir;

    /// <summary>
    /// Initializes test directory and file manager before each test.
    /// </summary>
    public FileManagmentTests()
    {
        _fileManager = new FileManagment();
        _testDir = Path.Combine(Path.GetTempPath(), "FileManagementTests");
        Directory.CreateDirectory(_testDir);
    }

    /// <summary>
    /// Cleans up temporary test directory after each test.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    private string GetTempFilePath(string name = "test.txt") => Path.Combine(_testDir, name);

    [Fact]
    /// <summary>
    /// Ensures filename without extension is correctly extracted from path.
    /// </summary>
    public void ExtractFileNameFromPath_ShouldReturnCorrectName()
    {
        string path = @"C:\folder\example.txt";
        string result = _fileManager.ExtractFileNameFromPath(path);

        Assert.Equal("example", result);
    }

    [Fact]
    /// <summary>
    /// Validates correct extension extraction from full file path.
    /// </summary>
    public void ExtractExtensionFileFromPath_ShouldReturnCorrectExtension()
    {
        string path = @"C:\folder\example.json";
        string result = _fileManager.ExtractExtensionFileFromPath(path);

        Assert.Equal("json", result);
    }

    [Fact]
    /// <summary>
    /// Validates creation of text file and correctness of written content.
    /// </summary>
    public void CreateFile_WithText_ShouldCreateFileAndWriteContent()
    {
        string path = GetTempFilePath();
        string content = "Hello xUnit!";
        _fileManager.CreateFile(path, content);

        Assert.True(File.Exists(path));
        Assert.Equal(content, File.ReadAllText(path));
    }

    [Fact]
    /// <summary>
    /// Validates creation of binary file using byte array.
    /// </summary>
    public void CreateFile_WithBytes_ShouldCreateBinaryFile()
    {
        string path = GetTempFilePath("binary.dat");
        byte[] data = Encoding.UTF8.GetBytes("Binary data");
        _fileManager.CreateFile(path, data);

        Assert.True(File.Exists(path));
    }

    [Fact]
    /// <summary>
    /// Confirms file listing by extension (.json).
    /// </summary>
    public void GetAllFilesByTypeFromPath_ShouldReturnJsonFiles()
    {
        string filePath = GetTempFilePath("data.json");
        File.WriteAllText(filePath, "{}");
        var files = _fileManager.GetAllFilesByTypeFromPath(_testDir, FileTypeEnum.Json);

        Assert.Contains(filePath, files);
    }

    [Fact]
    /// <summary>
    /// Confirms file duplication via copy operation.
    /// </summary>
    public void CopyFile_ShouldDuplicateFile()
    {
        string source = GetTempFilePath("source.txt");
        string target = GetTempFilePath("copied.txt");
        File.WriteAllText(source, "Copy me");
        _fileManager.CopyFile(source, target);

        Assert.True(File.Exists(target));
    }

    [Fact]
    /// <summary>
    /// Ensures file move (relocation) removes source and creates target.
    /// </summary>
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
    /// <summary>
    /// Confirms file deletion operation.
    /// </summary>
    public void DeleteFile_ShouldRemoveFile()
    {
        string path = GetTempFilePath("to_delete.txt");
        File.WriteAllText(path, "Remove me");
        _fileManager.DeleteFile(path);

        Assert.False(File.Exists(path));
    }

    [Fact]
    /// <summary>
    /// Ensures asynchronous file reading works as expected.
    /// </summary>
    public async Task ReadFileAsync_ShouldReadTextContent()
    {
        string path = GetTempFilePath("read.txt");
        string content = "Read me async";
        await File.WriteAllTextAsync(path, content);
        string result = await _fileManager.ReadFileAsync(path);

        Assert.Equal(content, result);
    }

    [Fact]
    /// <summary>
    /// Verifies writing multiple lines to a file asynchronously.
    /// </summary>
    public async Task WriteAllLinesAsync_ShouldWriteMultipleLines()
    {
        string path = GetTempFilePath("lines.txt");
        var lines = new[] { "Line 1", "Line 2", "Line 3" };
        await _fileManager.WriteAllLinesAsync(path, lines);
        var readLines = await File.ReadAllLinesAsync(path);

        Assert.Equal(lines, readLines);
    }

    [Fact]
    /// <summary>
    /// Confirms base64 encoding of file content.
    /// </summary>
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
    /// <summary>
    /// Validates base64 decoding to stream returns correct content.
    /// </summary>
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
    /// <summary>
    /// Should return empty content if file does not exist.
    /// </summary>
    public async Task ReadFileAsync_NonExistent_ShouldReturnEmpty()
    {
        string path = GetTempFilePath("notfound.txt");
        string result = await _fileManager.ReadFileAsync(path);

        Assert.Equal(string.Empty, result);
    }
    [Fact]
    /// <summary>
    /// Returns an empty stream when base64 input is invalid.
    /// </summary>
    public void DecodeBase64ToStream_InvalidInput_ShouldReturnEmptyStream()
    {
        var manager = new FileManagment();
        var result = manager.DecodeBase64ToStream("invalid_base64_%%%");

        Assert.NotNull(result);
        Assert.Equal(0, result.Length);
    }
}