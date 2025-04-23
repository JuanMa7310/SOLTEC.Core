using NUnit.Framework;
using SOLTEC.Core;
using System.Text;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class EncryptionIntegrationTests
    {
        private readonly Encryption _encryption = new();

        [Test]
        /// <summary>
        /// Validates that encoding and then decoding a string with Base64 preserves the original value.
        /// </summary>
        public void Base64EncodeDecode_ShouldPreserveOriginalString()
        {
            var _original = "Hello, Integration!";
            var _encoded = _encryption.Base64Encode(_original);
            var _decoded = _encryption.Base64Decode(_encoded);

            Assert.That(_decoded, Is.EqualTo(_original));
        }

        [Test]
        /// <summary>
        /// Ensures that SHA256 hash of a string returns a 64-character hexadecimal string.
        /// </summary>
        public void GenerateSHA256_ShouldReturn64CharHash()
        {
            var _input = "test-value";
            var _hash = _encryption.GenerateSHA256(_input);

            Assert.That(_hash.Length, Is.EqualTo(64));
            Assert.That(_hash, Does.Match("^[a-f0-9]{64}$"));
        }
    }
}
