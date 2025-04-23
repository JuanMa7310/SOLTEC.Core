using NUnit.Framework;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class ServiceResponseIntegrationTests
    {
        [Test]
        public void CreateSuccess_ShouldReturnSuccessTrue()
        {
            var _response = ServiceResponse.CreateSuccess("Operation completed");
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Message, Is.EqualTo("Operation completed"));
        }

        [Test]
        public void CreateError_ShouldReturnSuccessFalse()
        {
            var _response = ServiceResponse.CreateError("ERR001", "Error occurred");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.Key, Is.EqualTo("ERR001"));
            Assert.That(_response.Message, Is.EqualTo("Error occurred"));
        }

        [Test]
        public void CreateWarning_ShouldReturnWarningResponse()
        {
            var _response = ServiceResponse.CreateWarning(206, "Partial content");
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
            Assert.That(_response.Message, Is.EqualTo("Partial content"));
        }
    }
}
