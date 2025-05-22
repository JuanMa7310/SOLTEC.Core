using SOLTEC.Core.Adapters.Excel;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the DefaultFileOpener class.
/// </summary>
public class DefaultFileOpenerTests
{
    private string _tempFilePath;
    private DefaultFileOpener _opener;

    [SetUp]
    public void Setup()
    {
        _opener = new DefaultFileOpener();
        _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        File.WriteAllText(_tempFilePath, "TestContent");
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_tempFilePath))
            File.Delete(_tempFilePath);
    }

    [Test]
    /// <summary>
    /// Test that Open returns a readable stream for an existing file.
    /// </summary>
    public void Open_ExistingFile_ReturnsReadableStream()
    {
        using var stream = _opener.Open(_tempFilePath);
        Assert.That(stream, Is.Not.Null);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        Assert.That(content, Is.EqualTo("TestContent"));
    }
    [Test]
    /// <summary>
    /// Test that Open throws FileNotFoundException for a non-existent file.
    /// </summary>
    public void Open_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = _tempFilePath + ".doesnotexist";

        Assert.Throws<FileNotFoundException>(() => _opener.Open(nonExistentPath));
    }
}
