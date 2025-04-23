using NUnit.Framework;
using SOLTEC.Core.Connections.Exceptions;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class HttpCoreExceptionIntegrationTests
    {
        [Test]
        public void Constructor_ShouldAssignAllProperties()
        {
            var _exception = new HttpCoreException("CODE123", "Unauthorized access", HttpStatusCode.Unauthorized, "Access denied", HttpCoreErrorEnum.Unauthorized);

            Assert.That(_exception.Key, Is.EqualTo("CODE123"));
            Assert.That(_exception.Reason, Is.EqualTo("Unauthorized access"));
            Assert.That(_exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(_exception.ErrorMessage, Is.EqualTo("Access denied"));
            Assert.That(_exception.ErrorType, Is.EqualTo(HttpCoreErrorEnum.Unauthorized));
        }
    }
}
