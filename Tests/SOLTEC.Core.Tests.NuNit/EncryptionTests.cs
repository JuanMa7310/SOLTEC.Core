using SOLTEC.Core.Encryptions;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the Encryptions class using NUnit.
/// </summary>
public class EncryptionTests
{
    private readonly Encryption _encryption = new();

    /// <summary>
    /// Tests GenerateUniqueKey returns a string of specified length.
    /// Sends length 12 and expects 12 character result.
    /// </summary>
    [Test]
    public void GenerateUniqueKey_ReturnsCorrectLength()
    {
        var _result = _encryption.GenerateUniqueKey(12);

        Assert.That(_result.Length, Is.EqualTo(12));
    }

    /// <summary>
    /// Tests CreateTokenHMACSHA256 returns a valid base64 string.
    /// Sends message and secret, expects non-empty base64 string.
    /// </summary>
    [Test]
    public void CreateTokenHMACSHA256_ReturnsBase64()
    {
        var _token = _encryption.CreateTokenHMACSHA256("test", "secret");

        Assert.That(_token, Is.Not.Empty);
    }

    /// <summary>
    /// Tests Base64Encode and Base64Decode return original string.
    /// </summary>
    [Test]
    public void Base64EncodeDecode_RoundTrip_Success()
    {
        var _original = "hello world";
        var _encoded = _encryption.Base64Encode(_original);
        var _decoded = _encryption.Base64Decode(_encoded);

        Assert.That(_decoded, Is.EqualTo(_original));
    }

    /// <summary>
    /// Tests CreateMD5 returns 32-character hex string.
    /// </summary>
    [Test]
    public void CreateMD5_ReturnsHexHash()
    {
        var _hash = _encryption.CreateMD5("input");

        Assert.That(_hash.Length, Is.EqualTo(32));
    }

    /// <summary>
    /// Tests Encrypt and Decrypt round-trip using a password.
    /// </summary>
    [Test]
    public void EncryptDecrypt_RoundTrip_Success()
    {
        var _text = "Sensitive Info";
        var _password = "P@ssw0rd!";
        var _encrypted = _encryption.Encrypt(_text, _password);
        var _decrypted = _encryption.Decrypt(_encrypted!, _password);

        Assert.That(_decrypted, Is.EqualTo(_text));
    }
}