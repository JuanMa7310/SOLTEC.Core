using System.Security.Cryptography;
using System.Text;

namespace SOLTEC.Core;

public class Encryptions
{
    /// <summary>
    /// Generates a unique code with the length indicated by parameter
    /// </summary>
    /// <param name="maxSize">Maximum code length</param>
    /// <returns></returns>
    public virtual string GenerateUniqueKey(int maxSize = 10)
    {
        const string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var chars = a.ToCharArray();
        var size = maxSize;
        var data = new byte[1];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
        }
        StringBuilder result = new(size);
        foreach (var b in data) 
        { 
            result.Append(chars[b % (chars.Length - 1)]); 
        }

        return result.ToString();
    }

    /// <summary>
    /// Create an encrypted token for a message with the indicated secret
    /// </summary>
    /// <param name="message">Message to encode</param>
    /// <param name="secret">Secret</param>
    public virtual string CreateTokenHMACSHA256(string message, string secret)
    {
        secret ??= "";
        ASCIIEncoding encoding = new();
        var keyByte = encoding.GetBytes(secret);
        var messageBytes = encoding.GetBytes(message);

        using HMACSHA256 hmacsha256 = new(keyByte);
        var hashmessage = hmacsha256.ComputeHash(messageBytes);

        return Convert.ToBase64String(hashmessage);
    }

    /// <summary>
    /// Create an MD5 code based on the message passed by parameter
    /// </summary>
    /// <param name="message">Message to encode</param>
    public virtual string CreateMD5(string message)
    {
        ASCIIEncoding encoding = new();
        StringBuilder sb = new();
        var stream = MD5.HashData(encoding.GetBytes(message));

        foreach (var objByte in stream)
        {
            sb.AppendFormat("{0:x2}", objByte);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Create an MD5 from a GUID
    /// </summary>
    public virtual string Token()
    {
        var i = Guid.NewGuid().ToByteArray().Aggregate<byte, long>(1, (current, b) => current * (b + 1));

        return CreateMD5($"{i - DateTime.Now.Ticks:x}");
    }

    /// <summary>
    /// Decode a message
    /// </summary>
    /// <param name="input">Message to decode.</param>
    public virtual string Base64Decode(string input)
    {
        var output = Convert.FromBase64String(input);

        return Encoding.UTF8.GetString(output);
    }

    /// <summary>
    /// Decode a message
    /// </summary>
    /// <param name="input">Message to encode</param>
    public virtual string Base64Encode(string input)
    {
        var output = Encoding.UTF8.GetBytes(input);

        return Convert.ToBase64String(output);
    }

    /// <summary>
    /// Create an SHA1 code based on the message passed by parameter
    /// </summary>
    /// <param name="message">Message to encode</param>
    public virtual string GenerateSHA1(string message)
    {
        ASCIIEncoding encoding = new();
        StringBuilder sb = new();
        var stream = SHA1.HashData(encoding.GetBytes(message));

        foreach (var t in stream)
        {
            sb.AppendFormat("{0:x2}", t);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Create an SHA256 code based on the message passed by parameter
    /// </summary>
    /// <param name="message">Message to encode</param>
    public virtual string GenerateSHA256(string message)
    {
        ASCIIEncoding encoding = new();
        StringBuilder sb = new();
        var stream = SHA256.HashData(encoding.GetBytes(message));

        foreach (var t in stream)
        {
            sb.AppendFormat("{0:x2}", t);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Create an SHA384 code based on the message passed by parameter
    /// </summary>
    /// <param name="message">Message to encode</param>
    public virtual string GenerateSHA384(string message)
    {
        ASCIIEncoding encoding = new();
        StringBuilder sb = new();
        var stream = SHA384.HashData(encoding.GetBytes(message));

        foreach (var t in stream)
        {
            sb.AppendFormat("{0:x2}", t);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Create an SHA512 code based on the message passed by parameter
    /// </summary>
    /// <param name="message">Message to encode</param>
    public virtual string GenerateSHA512(string message)
    {
        ASCIIEncoding encoding = new();
        StringBuilder sb = new();
        var stream = SHA512.HashData(encoding.GetBytes(message));

        foreach (var t in stream)
        {
            sb.AppendFormat("{0:x2}", t);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Encrypt a string.
    /// </summary>
    /// <param name="data">String to be encrypted</param>
    /// <param name="password"></param>
    /// <exception cref="FormatException"></exception>
    public virtual string? Encrypt(string data, string password)
    {
        if (data == null) 
            return null;
        // Get the bytes of the string
        var bytesToBeEncrypted = Encoding.UTF8.GetBytes(data);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        // Hash the password with SHA256
        passwordBytes = SHA512.HashData(passwordBytes);
        using var aesAlg = Aes.Create();
        var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes, aesAlg.IV);

        return Convert.ToBase64String(bytesEncrypted);
    }

    /// <summary>
    /// Decrypt a string.
    /// </summary>
    /// <param name="encryptedData">String to be decrypted</param>
    /// <exception cref="FormatException"></exception>
    public virtual string? Decrypt(string encryptedData, string password)
    {
        if (encryptedData == null) 
            return null;
        // Get the bytes of the string
        var bytesToBeDecrypted = Convert.FromBase64String(encryptedData);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        passwordBytes = SHA512.HashData(passwordBytes);
        using var aesAlg = Aes.Create();
        var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes, aesAlg.IV);

        return Encoding.UTF8.GetString(bytesDecrypted);
    }

    private byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] key, byte[] IV)
    {
        // Check arguments.
        if (bytesToBeEncrypted is not { Length: > 0 })
            throw new ArgumentNullException(nameof(bytesToBeEncrypted));
        if (key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(key));
        if (IV is not { Length: > 0 })
            throw new ArgumentNullException(nameof(IV));

        // Create an Aes object with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = IV;

        // Create an encryptor to perform the stream transform
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        // Create the streams used for encryption.
        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
        {
            csEncrypt.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            csEncrypt.Close();
        }
        var encryptedBytes = msEncrypt.ToArray();

        // Return the encrypted bytes from the memory stream.
        return encryptedBytes;
    }
    private byte[] Decrypt(byte[] bytesToBeDencrypted, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (bytesToBeDencrypted is not { Length: > 0 })
            throw new ArgumentNullException(nameof(bytesToBeDencrypted));
        if (Key is not { Length: > 0 })
            throw new ArgumentNullException(nameof(Key));
        if (IV is not { Length: > 0 })
            throw new ArgumentNullException(nameof(IV));

        // Declare the string used to hold the decrypted text.

        // Create an Aes object
        // with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        // Create a decryptor to perform the stream transform.
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using MemoryStream msDecrypt = new();
        using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Write))
        {
            csDecrypt.Write(bytesToBeDencrypted, 0, bytesToBeDencrypted.Length);
            csDecrypt.Close();
        }

        // Read the decrypted bytes from the decrypting stream
        // and place them in a bytes array.
        var decyptedBytes = msDecrypt.ToArray();
        return decyptedBytes;
    }
}