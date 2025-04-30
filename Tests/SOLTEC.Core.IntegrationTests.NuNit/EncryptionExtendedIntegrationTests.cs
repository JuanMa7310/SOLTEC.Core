using NUnit.Framework;
using SOLTEC.Core.Encryptions;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class EncryptionExtendedIntegrationTests
    {
        private readonly Encryption _encryption = new();

        [Test]
        public void CreateMD5_ShouldReturn32CharHash()
        {
            var _input = "md5-test";
            var _hash = _encryption.CreateMD5(_input);

            Assert.That(_hash.Length, Is.EqualTo(32));
        }

        [Test]
        public void GenerateSHA1_ShouldReturn40CharHash()
        {
            var _input = "sha1";
            var _hash = _encryption.GenerateSHA1(_input);

            Assert.That(_hash.Length, Is.EqualTo(40));
        }

        [Test]
        public void GenerateSHA384_ShouldReturn96CharHash()
        {
            var _input = "sha384";
            var _hash = _encryption.GenerateSHA384(_input);

            Assert.That(_hash.Length, Is.EqualTo(96));
        }

        [Test]
        public void GenerateSHA512_ShouldReturn128CharHash()
        {
            var _input = "sha512";
            var _hash = _encryption.GenerateSHA512(_input);

            Assert.That(_hash.Length, Is.EqualTo(128));
        }

        [Test]
        public void CreateTokenHMACSHA256_ShouldReturnToken()
        {
            var _token = _encryption.CreateTokenHMACSHA256("secret", "payload");

            Assert.That(_token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Token_ShouldReturnExpectedToken()
        {
            var _token = _encryption.CreateTokenHMACSHA256("key", "payload");

            Assert.That(_token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void GenerateUniqueKey_ShouldReturnNonEmptyString()
        {
            var _key = _encryption.GenerateUniqueKey();

            Assert.That(_key, Is.Not.Null.And.Not.Empty);
        }
    }
}
