using SOLTEC.Core.Encryptions;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the Encryptions class using xUnit.
/// </summary>
public class EncryptionsTests
{
    private readonly Encryption _encryption = new();

    /// <summary>
    /// Tests GenerateUniqueKey returns a string of specified length.
    /// Sends length 12 and expects 12 character result.
    /// </summary>
    [Fact]
    public void GenerateUniqueKey_ReturnsCorrectLength()
    {
        var _result = _encryption.GenerateUniqueKey(12);

        Assert.Equal(12, _result.Length);
    }

    /// <summary>
    /// Tests CreateTokenHMACSHA256 returns a valid base64 string.
    /// Sends message and secret, expects non-empty base64 string.
    /// </summary>
    [Fact]
    public void CreateTokenHMACSHA256_ReturnsBase64()
    {
        var _token = _encryption.CreateTokenHMACSHA256("test", "secret");

        Assert.False(string.IsNullOrWhiteSpace(_token));
    }

    /// <summary>
    /// Tests Base64Encode and Base64Decode return original string.
    /// </summary>
    [Fact]
    public void Base64EncodeDecode_RoundTrip_Success()
    {
        var _original = "hello world";
        var _encoded = _encryption.Base64Encode(_original);
        var _decoded = _encryption.Base64Decode(_encoded);

        Assert.Equal(_original, _decoded);
    }

    /// <summary>
    /// Tests CreateMD5 returns 32-character hex string.
    /// </summary>
    [Fact]
    public void CreateMD5_ReturnsHexHash()
    {
        var _hash = _encryption.CreateMD5("input");

        Assert.Equal(32, _hash.Length);
    }

    /// <summary>
    /// Tests Encrypt and Decrypt round-trip using a password.
    /// </summary>
    [Fact]
    public void EncryptDecrypt_RoundTrip_Success()
    {
        var _text = "Sensitive Info";
        var _password = "P@ssw0rd!";
        var _encrypted = _encryption.Encrypt(_text, _password);
        var _decrypted = _encryption.Decrypt(_encrypted!, _password);

        Assert.Equal(_text, _decrypted);
    }
}
