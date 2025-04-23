using NUnit.Framework;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class ServiceResponseGenericIntegrationTests
    {
        [Test]
        public void CreateSuccess_ShouldSetResultAndSuccess()
        {
            var _response = ServiceResponse<string>.CreateSuccess("TestResult", "Operation done");

            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Result, Is.EqualTo("TestResult"));
            Assert.That(_response.Message, Is.EqualTo("Operation done"));
        }

        [Test]
        public void CreateError_ShouldReturnWithKeyAndMessage()
        {
            var _response = ServiceResponse<string>.CreateError("ERROR_KEY", "Invalid state");

            Assert.That(_response.Success, Is.False);
            Assert.That(_response.Key, Is.EqualTo("ERROR_KEY"));
            Assert.That(_response.Message, Is.EqualTo("Invalid state"));
            Assert.That(_response.Result, Is.Null);
        }

        [Test]
        public void CreateWarning_ShouldSetCodeMessageAndNullResult()
        {
            var _response = ServiceResponse<string>.CreateWarning(299, "Partial result");

            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(299));
            Assert.That(_response.Message, Is.EqualTo("Partial result"));
            Assert.That(_response.Result, Is.Null);
        }
    }
}
