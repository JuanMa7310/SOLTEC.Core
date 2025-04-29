using SOLTEC.Core.Encryptions;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the Encryptions class using NUnit.
/// </summary>
public class EncryptionTests
{
    private readonly Encryption _encryption = new();

    [Test]
    /// <summary>
    /// Tests GenerateUniqueKey returns a string of specified length.
    /// Sends length 12 and expects 12 character result.
    /// </summary>
    public void GenerateUniqueKey_ReturnsCorrectLength()
    {
        var _result = _encryption.GenerateUniqueKey(12);

        Assert.That(_result.Length, Is.EqualTo(12));
    }

    [Test]
    /// <summary>
    /// Tests CreateTokenHMACSHA256 returns a valid base64 string.
    /// Sends message and secret, expects non-empty base64 string.
    /// </summary>
    public void CreateTokenHMACSHA256_ReturnsBase64()
    {
        var _token = _encryption.CreateTokenHMACSHA256("test", "secret");

        Assert.That(_token, Is.Not.Empty);
    }

    [Test]
    /// <summary>
    /// Tests Base64Encode and Base64Decode return original string.
    /// </summary>
    public void Base64EncodeDecode_RoundTrip_Success()
    {
        var _original = "hello world";
        var _encoded = _encryption.Base64Encode(_original);
        var _decoded = _encryption.Base64Decode(_encoded);

        Assert.That(_decoded, Is.EqualTo(_original));
    }

    [Test]
    /// <summary>
    /// Tests CreateMD5 returns 32-character hex string.
    /// </summary>
    public void CreateMD5_ReturnsHexHash()
    {
        var _hash = _encryption.CreateMD5("input");

        Assert.That(_hash.Length, Is.EqualTo(32));
    }

    [Test]
    /// <summary>
    /// Tests Encrypt and Decrypt round-trip using a password.
    /// </summary>
    public void EncryptDecrypt_RoundTrip_Success()
    {
        var _text = "Sensitive Info";
        var _password = "P@ssw0rd!";
        var _encrypted = _encryption.Encrypt(_text, _password);
        var _decrypted = _encryption.Decrypt(_encrypted!, _password);

        Assert.That(_decrypted, Is.EqualTo(_text));
    }

    [Test]
    /// <summary>
    /// Tests GenerateSHA1 returns a 40-character hexadecimal hash for a known input.
    /// Passes the string "data" and expects a SHA1 hash string of length 40.
    /// </summary>
    public void GenerateSHA1_ReturnsSHA1Hash()
    {
        var _hash = _encryption.GenerateSHA1("data");

        Assert.That(_hash.Length, Is.EqualTo(40));
        Assert.That(_hash, Does.Match("^[a-f0-9]{40}$"));
    }

    [Test]
    /// <summary>
    /// Tests GenerateSHA256 returns a 64-character hexadecimal hash.
    /// Passes "secure" and checks for expected format and length.
    /// </summary>
    public void GenerateSHA256_ReturnsSHA256Hash()
    {
        var _hash = _encryption.GenerateSHA256("secure");

        Assert.That(_hash.Length, Is.EqualTo(64));
        Assert.That(_hash, Does.Match("^[a-f0-9]{64}$"));
    }

    [Test]
    /// <summary>
    /// Tests GenerateSHA384 returns a 96-character hexadecimal string.
    /// Passes "text" and ensures correct length and hexadecimal format.
    /// </summary>
    public void GenerateSHA384_ReturnsSHA384Hash()
    {
        var _hash = _encryption.GenerateSHA384("text");

        Assert.That(_hash.Length, Is.EqualTo(96));
        Assert.That(_hash, Does.Match("^[a-f0-9]{96}$"));
    }

    [Test]
    /// <summary>
    /// Tests GenerateSHA512 returns a 128-character hexadecimal string.
    /// Passes "longinput" and verifies format and hash length.
    /// </summary>
    public void GenerateSHA512_ReturnsSHA512Hash()
    {
        var _hash = _encryption.GenerateSHA512("longinput");

        Assert.That(_hash.Length, Is.EqualTo(128));
        Assert.That(_hash, Does.Match("^[a-f0-9]{128}$"));
    }
}