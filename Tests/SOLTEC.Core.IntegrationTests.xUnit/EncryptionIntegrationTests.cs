using Xunit;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class EncryptionIntegrationTests
    {
        private readonly Encryption _encryption = new();

        [Fact]
        /// <summary>
        /// Ensures that Base64 encoding and decoding restores the original input.
        /// </summary>
        public void Base64EncodeDecode_ShouldMatchOriginal()
        {
            var _original = "Hello, Integration!";
            var _encoded = _encryption.Base64Encode(_original);
            var _decoded = _encryption.Base64Decode(_encoded);

            Assert.Equal(_original, _decoded);
        }

        [Fact]
        /// <summary>
        /// Verifies that SHA256 hashing returns a valid 64-character hex string.
        /// </summary>
        public void GenerateSHA256_ShouldBeValidLength()
        {
            var _input = "test-value";
            var _hash = _encryption.GenerateSHA256(_input);

            Assert.Equal(64, _hash.Length);
            Assert.Matches("^[a-f0-9]{64}$", _hash);
        }
    }
}
