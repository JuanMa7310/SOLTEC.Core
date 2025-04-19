using System.Security.Cryptography;
using System.Text;

namespace SOLTEC.Core.Encryptions;

/// <summary>
/// Provides utility methods for various encryption, hashing, and encoding operations.
/// </summary>
/// <example>
/// Example of generating a SHA256 hash:
/// <code>
/// var hash = new Encryptions().GenerateSHA256("hello world");
/// </code>
/// </example>
public class Encryption
{
    /// <summary>
    /// Generates a random alphanumeric string of specified length.
    /// </summary>
    /// <param name="maxSize">The length of the generated key.</param>
    public virtual string GenerateUniqueKey(int maxSize = 10)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var _data = new byte[maxSize];
        using var _rng = RandomNumberGenerator.Create();
        _rng.GetBytes(_data);
        var _result = new StringBuilder(maxSize);

        foreach (var _b in _data)
        {
            _result.Append(chars[_b % chars.Length]);
        }

        return _result.ToString();
    }

    /// <summary>
    /// Creates an HMAC SHA256 token using the given message and secret.
    /// </summary>
    /// <param name="message">The message to encode.</param>
    /// <param name="secret">The secret key used for encryption.</param>
    public virtual string CreateTokenHMACSHA256(string message, string secret)
    {
        var _key = Encoding.ASCII.GetBytes(secret ?? string.Empty);
        var _messageBytes = Encoding.ASCII.GetBytes(message);
        using var _hmac = new HMACSHA256(_key);
        var _hash = _hmac.ComputeHash(_messageBytes);

        return Convert.ToBase64String(_hash);
    }

    /// <summary>
    /// Generates an MD5 hash from a given message.
    /// </summary>
    /// <param name="message">The input string.</param>
    public virtual string CreateMD5(string message)
    {
        var _inputBytes = Encoding.ASCII.GetBytes(message);
        var _hashBytes = MD5.HashData(_inputBytes);

        return Convert.ToHexString(_hashBytes).ToLower();
    }

    /// <summary>
    /// Generates a unique MD5 token from a new GUID.
    /// </summary>
    public virtual string Token()
    {
        long _i = 1;


        foreach (byte _b in Guid.NewGuid().ToByteArray())
        {
            _i *= _b + 1;
        }

        return CreateMD5((_i - DateTime.Now.Ticks).ToString("x"));
    }

    /// <summary>
    /// Decodes a base64 string.
    /// </summary>
    /// <param name="input">The encoded string.</param>
    public virtual string Base64Decode(string input)
    {
        var _output = Convert.FromBase64String(input);

        return Encoding.UTF8.GetString(_output);
    }

    /// <summary>
    /// Encodes a string into base64.
    /// </summary>
    /// <param name="input">The input string.</param>
    public virtual string Base64Encode(string input)
    {
        var _bytes = Encoding.UTF8.GetBytes(input);

        return Convert.ToBase64String(_bytes);
    }

    /// <summary>
    /// Generates an SHA1 hash from a given message.
    /// </summary>
    /// <param name="message">The input string.</param>
    public virtual string GenerateSHA1(string message)
    {
        var _bytes = SHA1.HashData(Encoding.ASCII.GetBytes(message));

        return Convert.ToHexString(_bytes).ToLower();
    }

    /// <summary>
    /// Generates an SHA256 hash from a given message.
    /// </summary>
    /// <param name="message">The input string.</param>
    public virtual string GenerateSHA256(string message)
    {
        var _bytes = SHA256.HashData(Encoding.ASCII.GetBytes(message));

        return Convert.ToHexString(_bytes).ToLower();
    }

    /// <summary>
    /// Generates an SHA384 hash from a given message.
    /// </summary>
    /// <param name="message">The input string.</param>
    public virtual string GenerateSHA384(string message)
    {
        var _bytes = SHA384.HashData(Encoding.ASCII.GetBytes(message));

        return Convert.ToHexString(_bytes).ToLower();
    }

    /// <summary>
    /// Generates an SHA512 hash from a given message.
    /// </summary>
    /// <param name="message">The input string.</param>
    public virtual string GenerateSHA512(string message)
    {
        var _bytes = SHA512.HashData(Encoding.ASCII.GetBytes(message));

        return Convert.ToHexString(_bytes).ToLower();
    }

    /// <summary>
    /// Encrypts a string using a password.
    /// </summary>
    /// <param name="data">The string to encrypt.</param>
    /// <param name="password">The password for encryption.</param>
    public virtual string? Encrypt(string data, string password)
    {
        if (data == null) return null;
        var _toEncrypt = Encoding.UTF8.GetBytes(data);
        var _password = SHA512.HashData(Encoding.UTF8.GetBytes(password));
        var _encrypted = Encrypt(_toEncrypt, _password);

        return Convert.ToBase64String(_encrypted);
    }

    /// <summary>
    /// Decrypts a base64 encoded string using a password.
    /// </summary>
    /// <param name="encryptedData">The encrypted base64 string.</param>
    /// <param name="password">The password used for encryption.</param>
    public virtual string? Decrypt(string encryptedData, string password)
    {
        if (encryptedData == null) return null;
        var _toDecrypt = Convert.FromBase64String(encryptedData);
        var _password = SHA512.HashData(Encoding.UTF8.GetBytes(password));
        var _decrypted = Decrypt(_toDecrypt, _password);

        return Encoding.UTF8.GetString(_decrypted);
    }

    private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] _encrypted;
        var _salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using var _aes = Aes.Create();
        using var _key = new Rfc2898DeriveBytes(passwordBytes, _salt, 1000, HashAlgorithmName.SHA256);

        _aes.KeySize = 256;
        _aes.BlockSize = 128;
        _aes.Key = _key.GetBytes(_aes.KeySize / 8);
        _aes.IV = _key.GetBytes(_aes.BlockSize / 8);
        _aes.Mode = CipherMode.CBC;

        using var _ms = new MemoryStream();
        using (var _cs = new CryptoStream(_ms, _aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            _cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        }
        _encrypted = _ms.ToArray();

        return _encrypted;
    }

    private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] _decrypted;
        var _salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using var _aes = Aes.Create();
        using var _key = new Rfc2898DeriveBytes(passwordBytes, _salt, 1000, HashAlgorithmName.SHA256);

        _aes.KeySize = 256;
        _aes.BlockSize = 128;
        _aes.Key = _key.GetBytes(_aes.KeySize / 8);
        _aes.IV = _key.GetBytes(_aes.BlockSize / 8);
        _aes.Mode = CipherMode.CBC;

        using var _ms = new MemoryStream();
        using (var _cs = new CryptoStream(_ms, _aes.CreateDecryptor(), CryptoStreamMode.Write))
        {
            _cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
        }
        _decrypted = _ms.ToArray();

        return _decrypted;
    }
}
