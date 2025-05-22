using SOLTEC.Core.Adapters.Excel;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the DefaultFileOpener class using xUnit.
/// </summary>
public class DefaultFileOpenerTests : IDisposable
{
    private readonly string _tempFilePath;
    private readonly DefaultFileOpener _opener;

    /// <summary>
    /// Constructor sets up a temporary file for testing.
    /// </summary>
    public DefaultFileOpenerTests()
    {
        _opener = new DefaultFileOpener();
        _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");
        File.WriteAllText(_tempFilePath, "TestContent");
    }

    /// <summary>
    /// Disposes resources by deleting the temporary file.
    /// </summary>
    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
            File.Delete(_tempFilePath);
    }

    [Fact]
    /// <summary>
    /// Test that Open returns a readable stream for an existing file.
    /// </summary>
    public void Open_ExistingFile_ReturnsReadableStream()
    {
        using var stream = _opener.Open(_tempFilePath);
        Assert.NotNull(stream);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        Assert.Equal("TestContent", content);
    }
    [Fact]
    /// <summary>
    /// Test that Open throws FileNotFoundException for a non-existent file.
    /// </summary>
    public void Open_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentPath = _tempFilePath + ".doesnotexist";
        Assert.Throws<FileNotFoundException>(() => _opener.Open(nonExistentPath));
    }
}
