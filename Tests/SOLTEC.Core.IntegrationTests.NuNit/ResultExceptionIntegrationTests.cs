using NUnit.Framework;
using SOLTEC.Core;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class ResultExceptionIntegrationTests
    {
        [Test]
        public void Constructor_WithAllValues_AssignsPropertiesCorrectly()
        {
            var _exception = new ResultException("ERR001", "Invalid operation", HttpStatusCode.BadRequest, "Invalid input");

            Assert.That(_exception.Key, Is.EqualTo("ERR001"));
            Assert.That(_exception.Reason, Is.EqualTo("Invalid operation"));
            Assert.That(_exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_exception.ErrorMessage, Is.EqualTo("Invalid input"));
        }
    }
}
