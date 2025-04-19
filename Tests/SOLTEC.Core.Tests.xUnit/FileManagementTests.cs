using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

public class FileManagementTests : IDisposable
{
    private readonly FileManagement _fileManager;
    private readonly string _testFilePath;
    private readonly string _testDir;

    public FileManagementTests()
    {
        _fileManager = new FileManagement();
        _testDir = Path.Combine(Path.GetTempPath(), "FileManagementTests");
        Directory.CreateDirectory(_testDir);
        _testFilePath = Path.Combine(_testDir, "test.txt");
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    [Fact]
    public void ExtractFileNameFromPath_ValidPath_ReturnsFileName()
    {
        string path = @"C:\folder\file.txt";
        string result = _fileManager.ExtractFileNameFromPath(path);
        Assert.Equal("file", result);
    }

    [Fact]
    public void ExtractExtensionFileFromPath_ValidPath_ReturnsExtension()
    {
        string path = @"C:\folder\file.txt";
        string result = _fileManager.ExtractExtensionFileFromPath(path);
        Assert.Equal("txt", result);
    }

    [Fact]
    public void CreateFile_WithStringContent_CreatesFile()
    {
        string content = "Hello world!";
        _fileManager.CreateFile(_testFilePath, content);
        Assert.True(File.Exists(_testFilePath));
        Assert.Equal(content, File.ReadAllText(_testFilePath));
    }

    [Fact]
    public void CreateFile_WithBinaryContent_CreatesFile()
    {
        byte[] data = Encoding.UTF8.GetBytes("Test binary");
        _fileManager.CreateFile(_testFilePath, data);
        Assert.True(File.Exists(_testFilePath));
    }

    [Fact]
    public async Task ReadFileAsync_ReturnsCorrectContent()
    {
        string expected = "Async test";
        await File.WriteAllTextAsync(_testFilePath, expected);
        string result = await _fileManager.ReadFileAsync(_testFilePath);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ConvertFileToBase64Async_ValidFile_ReturnsBase64()
    {
        string content = "Base64 test";
        await File.WriteAllTextAsync(_testFilePath, content);
        string base64 = await _fileManager.ConvertFileToBase64Async(_testFilePath);
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        Assert.Equal(content, decoded);
    }

    [Fact]
    public void DecodeBase64ToStream_ValidString_ReturnsStream()
    {
        string original = "Stream content";
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
        using var stream = _fileManager.DecodeBase64ToStream(base64);
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();
        Assert.Equal(original, result);
    }
}
