using SOLTEC.Core.Enums;
using System.Text;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the <see cref="FileManagment"/> class using xUnit.
/// Validates behavior for file creation, reading, deletion, and conversion.
/// </summary>
public class FileManagmentTests
{
    private FileManagment _fileManager;
    private string _testDir;

    [SetUp]
    /// <summary>
    /// Initializes a temporary directory and FileManagement instance before each test.
    /// </summary>
    public void SetUp()
    {
        _fileManager = new FileManagment();
        _testDir = Path.Combine(Path.GetTempPath(), "FileManagementTests_NUnit");
        Directory.CreateDirectory(_testDir);
    }

    [TearDown]
    /// <summary>
    /// Cleans up the temporary directory after each test.
    /// </summary>
    public void TearDown()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    private string GetTempFilePath(string name = "test.txt") => Path.Combine(_testDir, name);

    [Test]
    /// <summary>
    /// Verifies that the filename is extracted correctly from a full path.
    /// </summary>
    public void ExtractFileNameFromPath_ShouldReturnCorrectName()
    {
        var result = _fileManager.ExtractFileNameFromPath(@"C:\folder\example.txt");

        Assert.That(result, Is.EqualTo("example"));
    }

    [Test]
    /// <summary>
    /// Verifies that the file extension is extracted correctly from a full path.
    /// </summary>
    public void ExtractExtensionFileFromPath_ShouldReturnCorrectExtension()
    {
        var result = _fileManager.ExtractExtensionFileFromPath(@"C:\folder\example.json");

        Assert.That(result, Is.EqualTo("json"));
    }

    [Test]
    /// <summary>
    /// Verifies that a file is created and text content is written correctly.
    /// </summary>
    public void CreateFile_WithText_ShouldCreateFileAndWriteContent()
    {
        string path = GetTempFilePath();
        string content = "Hello NUnit!";
        _fileManager.CreateFile(path, content);

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(path));
            Assert.That(File.ReadAllText(path), Is.EqualTo(content));
        });
    }

    [Test]
    /// <summary>
    /// Verifies that a binary file is created from byte array input.
    /// </summary>
    public void CreateFile_WithBytes_ShouldCreateBinaryFile()
    {
        string path = GetTempFilePath("binary.dat");
        byte[] data = Encoding.UTF8.GetBytes("Binary data");
        _fileManager.CreateFile(path, data);

        Assert.That(File.Exists(path));
    }

    [Test]
    /// <summary>
    /// Verifies that JSON files are correctly retrieved from a directory.
    /// </summary>
    public void GetAllFilesByTypeFromPath_ShouldReturnJsonFiles()
    {
        string filePath = GetTempFilePath("data.json");
        File.WriteAllText(filePath, "{}");
        var files = _fileManager.GetAllFilesByTypeFromPath(_testDir, FileTypeEnum.Json);

        Assert.That(files, Does.Contain(filePath));
    }

    [Test]
    /// <summary>
    /// Verifies that a file is successfully copied to a new location.
    /// </summary>
    public void CopyFile_ShouldDuplicateFile()
    {
        string source = GetTempFilePath("source.txt");
        string target = GetTempFilePath("copied.txt");
        File.WriteAllText(source, "Copy me");
        _fileManager.CopyFile(source, target);

        Assert.That(File.Exists(target));
    }

    [Test]
    /// <summary>
    /// Verifies that a file is successfully moved to a new location.
    /// </summary>
    public void MoveFile_ShouldRelocateFile()
    {
        string source = GetTempFilePath("to_move.txt");
        string target = GetTempFilePath("moved.txt");
        File.WriteAllText(source, "Move me");
        _fileManager.MoveFile(source, target);

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(target));
            Assert.That(File.Exists(source), Is.False);
        });
    }

    [Test]
    /// <summary>
    /// Verifies that a file is deleted successfully.
    /// </summary>
    public void DeleteFile_ShouldRemoveFile()
    {
        string path = GetTempFilePath("to_delete.txt");
        File.WriteAllText(path, "Remove me");
        _fileManager.DeleteFile(path);

        Assert.That(File.Exists(path), Is.False);
    }

    [Test]
    /// <summary>
    /// Verifies that a file's text content is read asynchronously.
    /// </summary>
    public async Task ReadFileAsync_ShouldReadTextContent()
    {
        string path = GetTempFilePath("read.txt");
        string content = "Read me async";
        await File.WriteAllTextAsync(path, content);
        var result = await _fileManager.ReadFileAsync(path);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    /// <summary>
    /// Verifies that multiple lines are written and read correctly from a file.
    /// </summary>
    public async Task WriteAllLinesAsync_ShouldWriteMultipleLines()
    {
        string path = GetTempFilePath("lines.txt");
        var lines = new[] { "Line 1", "Line 2", "Line 3" };
        await _fileManager.WriteAllLinesAsync(path, lines);
        var readLines = await File.ReadAllLinesAsync(path);

        Assert.That(readLines, Is.EqualTo(lines));
    }

    [Test]
    /// <summary>
    /// Verifies that a file is correctly encoded to Base64 string.
    /// </summary>
    public async Task ConvertFileToBase64Async_ShouldEncodeContent()
    {
        string path = GetTempFilePath("base64.txt");
        string content = "Encode me";
        await File.WriteAllTextAsync(path, content);
        string base64 = await _fileManager.ConvertFileToBase64Async(path);
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        Assert.That(decoded, Is.EqualTo(content));
    }

    [Test]
    /// <summary>
    /// Verifies that a valid Base64 string is converted into its original stream.
    /// </summary>
    public void DecodeBase64ToStream_ShouldReturnOriginalStream()
    {
        string original = "Stream this";
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(original));
        using var stream = _fileManager.DecodeBase64ToStream(base64);
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        Assert.That(result, Is.EqualTo(original));
    }

    [Test]
    /// <summary>
    /// Verifies that reading a non-existent file returns an empty string.
    /// </summary>
    public async Task ReadFileAsync_NonExistent_ShouldReturnEmpty()
    {
        string path = GetTempFilePath("notfound.txt");
        string result = await _fileManager.ReadFileAsync(path);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    /// <summary>
    /// Verifies that an invalid Base64 string results in an empty stream.
    /// </summary>
    public void DecodeBase64ToStream_InvalidInput_ShouldReturnEmptyStream()
    {
        var stream = _fileManager.DecodeBase64ToStream("Invalid base64!");

        Assert.That(stream.Length, Is.EqualTo(0));
    }
}