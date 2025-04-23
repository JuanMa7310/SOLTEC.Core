using Xunit;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class EncryptionExtendedIntegrationTests
    {
        private readonly Encryption _encryption = new();

        [Fact]
        public void CreateMD5_ShouldReturnValidHash()
        {
            var _hash = _encryption.CreateMD5("md5-test");
            Assert.Equal(32, _hash.Length);
        }

        [Fact]
        public void GenerateSHA1_ShouldReturnExpectedLength()
        {
            var _hash = _encryption.GenerateSHA1("sha1");
            Assert.Equal(40, _hash.Length);
        }

        [Fact]
        public void GenerateSHA384_ShouldReturnExpectedLength()
        {
            var _hash = _encryption.GenerateSHA384("sha384");
            Assert.Equal(96, _hash.Length);
        }

        [Fact]
        public void GenerateSHA512_ShouldReturnExpectedLength()
        {
            var _hash = _encryption.GenerateSHA512("sha512");
            Assert.Equal(128, _hash.Length);
        }

        [Fact]
        public void CreateTokenHMACSHA256_ShouldGenerateToken()
        {
            var _token = _encryption.CreateTokenHMACSHA256("secret", "payload");
            Assert.False(string.IsNullOrEmpty(_token));
        }

        [Fact]
        public void Token_ShouldGenerateToken()
        {
            var _token = _encryption.Token("key", "payload");
            Assert.False(string.IsNullOrEmpty(_token));
        }

        [Fact]
        public void GenerateUniqueKey_ShouldReturnString()
        {
            var _key = _encryption.GenerateUniqueKey();
            Assert.False(string.IsNullOrEmpty(_key));
        }
    }
}
