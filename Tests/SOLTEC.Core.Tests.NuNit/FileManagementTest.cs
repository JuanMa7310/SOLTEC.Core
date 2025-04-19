using System.Text;

namespace SOLTEC.Core.Tests.NuNit;

public class FileManagementTests
{
    private FileManagement _fileManager;
    private string _testFilePath;
    private string _testDir;

    [SetUp]
    public void Setup()
    {
        _fileManager = new FileManagement();
        _testDir = Path.Combine(Path.GetTempPath(), "FileManagementNUnit");
        Directory.CreateDirectory(_testDir);
        _testFilePath = Path.Combine(_testDir, "test.txt");
    }

    [TearDown]
    public void Cleanup()
    {
        if (Directory.Exists(_testDir))
            Directory.Delete(_testDir, true);
    }

    [Test]
    public void ExtractFileNameFromPath_ReturnsCorrectName()
    {
        var result = _fileManager.ExtractFileNameFromPath(@"C:\temp\example.txt");

        Assert.That(result, Is.EqualTo("example"));
    }

    [Test]
    public void ExtractExtensionFileFromPath_ReturnsCorrectExtension()
    {
        var result = _fileManager.ExtractExtensionFileFromPath(@"C:\temp\example.json");

        Assert.That(result, Is.EqualTo("json"));
    }

    [Test]
    public void CreateFile_WithText_WritesCorrectContent()
    {
        string content = "NUnit test content";
        _fileManager.CreateFile(_testFilePath, content);
        Assert.IsTrue(File.Exists(_testFilePath));

        Assert.That(File.ReadAllText(_testFilePath), Is.EqualTo(content));
    }

    [Test]
    public void CreateFile_WithBytes_WritesCorrectly()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("NUnit binary");
        _fileManager.CreateFile(_testFilePath, bytes);
        Assert.IsTrue(File.Exists(_testFilePath));
    }

    [Test]
    public async Task ReadFileAsync_ReadsCorrectly()
    {
        string content = "Read async";
        await File.WriteAllTextAsync(_testFilePath, content);
        var result = await _fileManager.ReadFileAsync(_testFilePath);

        Assert.That(result, Is.EqualTo(content));
    }

    [Test]
    public async Task ConvertFileToBase64Async_EncodesCorrectly()
    {
        string content = "NUnit base64";
        await File.WriteAllTextAsync(_testFilePath, content);
        string base64 = await _fileManager.ConvertFileToBase64Async(_testFilePath);
        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        Assert.That(decoded, Is.EqualTo(content));
    }

    [Test]
    public void DecodeBase64ToStream_DecodesCorrectly()
    {
        string text = "Base64 decoded";
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        using var stream = _fileManager.DecodeBase64ToStream(base64);
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();

        Assert.That(result, Is.EqualTo(text));
    }
}
