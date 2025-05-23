﻿using SOLTEC.Core.Encryptions;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the Encryptions class using xUnit.
/// </summary>
public class EncryptionTests
{
    private readonly Encryption _encryption = new();

    [Fact]
    /// <summary>
    /// Tests GenerateUniqueKey returns a string of specified length.
    /// Sends length 12 and expects 12 character result.
    /// </summary>
    public void GenerateUniqueKey_ReturnsCorrectLength()
    {
        var _result = _encryption.GenerateUniqueKey(12);

        Assert.Equal(12, _result.Length);
    }

    [Fact]
    /// <summary>
    /// Tests CreateTokenHMACSHA256 returns a valid base64 string.
    /// Sends message and secret, expects non-empty base64 string.
    /// </summary>
    public void CreateTokenHMACSHA256_ReturnsBase64()
    {
        var _token = _encryption.CreateTokenHMACSHA256("test", "secret");

        Assert.False(string.IsNullOrWhiteSpace(_token));
    }

    [Fact]
    /// <summary>
    /// Tests Base64Encode and Base64Decode return original string.
    /// </summary>
    public void Base64EncodeDecode_RoundTrip_Success()
    {
        var _original = "hello world";
        var _encoded = _encryption.Base64Encode(_original);
        var _decoded = _encryption.Base64Decode(_encoded);

        Assert.Equal(_original, _decoded);
    }

    [Fact]
    /// <summary>
    /// Tests CreateMD5 returns 32-character hex string.
    /// </summary>
    public void CreateMD5_ReturnsHexHash()
    {
        var _hash = _encryption.CreateMD5("input");

        Assert.Equal(32, _hash.Length);
    }

    [Fact]
    /// <summary>
    /// Tests Encrypt and Decrypt round-trip using a password.
    /// </summary>
    public void EncryptDecrypt_RoundTrip_Success()
    {
        var _text = "Sensitive Info";
        var _password = "P@ssw0rd!";
        var _encrypted = _encryption.Encrypt(_text, _password);
        var _decrypted = _encryption.Decrypt(_encrypted!, _password);

        Assert.Equal(_text, _decrypted);
    }

    [Fact]
    /// <summary>
    /// Verifies that GenerateSHA1 produces a 40-character lowercase hex hash for the string "data".
    /// </summary>
    public void GenerateSHA1_ShouldReturnCorrectHashLength()
    {
        var _hash = _encryption.GenerateSHA1("data");

        Assert.Equal(40, _hash.Length);
        Assert.Matches("^[a-f0-9]{40}$", _hash);
    }

    [Fact]
    /// <summary>
    /// Verifies that GenerateSHA256 produces a valid 64-character hash for "secure".
    /// </summary>
    public void GenerateSHA256_ShouldReturnValidHash()
    {
        var _hash = _encryption.GenerateSHA256("secure");

        Assert.Equal(64, _hash.Length);
        Assert.Matches("^[a-f0-9]{64}$", _hash);
    }

    [Fact]
    /// <summary>
    /// Verifies that GenerateSHA384 returns a 96-character hash when passed "text".
    /// </summary>
    public void GenerateSHA384_ShouldReturnValidHash()
    {
        var _hash = _encryption.GenerateSHA384("text");

        Assert.Equal(96, _hash.Length);
        Assert.Matches("^[a-f0-9]{96}$", _hash);
    }

    [Fact]
    /// <summary>
    /// Verifies that GenerateSHA512 returns a 128-character hash for the input "longinput".
    /// </summary>
    public void GenerateSHA512_ShouldReturnValidHash()
    {
        var _hash = _encryption.GenerateSHA512("longinput");

        Assert.Equal(128, _hash.Length);
        Assert.Matches("^[a-f0-9]{128}$", _hash);
    }
}
