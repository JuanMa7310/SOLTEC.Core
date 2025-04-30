using NUnit.Framework;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    /// <summary>
    /// Integration tests for the non-generic ServiceResponse class covering all public static factory methods.
    /// </summary>
    public class ServiceResponseIntegrationTests
    {
        [Test]
        /// <summary>
        /// Tests CreateSuccess(int).
        /// Expects a successful response with code 200 and no warnings.
        /// </summary>
        public void CreateSuccess_WithIntCode_ShouldReturnValidResponse()
        {
            var _response = ServiceResponse.CreateSuccess(200);
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
            Assert.That(_response.WarningMessages, Is.Null);
        }

        [Test]
        /// <summary>
        /// Tests CreateSuccess(HttpStatusCode).
        /// </summary>
        public void CreateSuccess_WithHttpCode_ShouldReturnValidResponse()
        {
            var _response = ServiceResponse.CreateSuccess(HttpStatusCode.OK);
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
        }

        [Test]
        /// <summary>
        /// Tests CreateSuccess with warnings.
        /// </summary>
        public void CreateSuccess_WithWarnings_ShouldReturnValidResponse()
        {
            var _warnings = new[] { "Delay notice" };
            var _response = ServiceResponse.CreateSuccess(200, _warnings);
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(int, message).
        /// </summary>
        public void CreateError_WithIntCode_ShouldReturnErrorResponse()
        {
            var _response = ServiceResponse.CreateError(500, "Internal Error");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo(500));
            Assert.That(_response.ErrorMessage, Is.EqualTo("Internal Error"));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(HttpStatusCode, message).
        /// </summary>
        public void CreateError_WithHttpCode_ShouldReturnErrorResponse()
        {
            var _response = ServiceResponse.CreateError(HttpStatusCode.InternalServerError, "Internal Error");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo(500));
            Assert.That(_response.ErrorMessage, Is.EqualTo("Internal Error"));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(int, message, warnings).
        /// </summary>
        public void CreateError_WithWarnings_ShouldReturnErrorResponse()
        {
            var _warnings = new[] { "Deprecated API" };
            var _response = ServiceResponse.CreateError(500, "Internal Error", _warnings);
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning(int, message).
        /// </summary>
        public void CreateWarning_WithIntCode_ShouldReturnWarningResponse()
        {
            var _response = ServiceResponse.CreateWarning(206, "Partial result");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ErrorMessage, Is.EqualTo("Partial result"));
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning(HttpStatusCode, message).
        /// </summary>
        public void CreateWarning_WithHttpCode_ShouldReturnWarningResponse()
        {
            var _response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ErrorMessage, Is.EqualTo("Partial result"));
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning(int, message, warnings).
        /// </summary>
        public void CreateWarning_WithWarnings_ShouldReturnWarningResponse()
        {
            var _warnings = new[] { "Quota near limit" };
            var _response = ServiceResponse.CreateWarning(206, "Partial", _warnings);
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning(HttpStatusCode, message, warnings).
        /// </summary>
        public void CreateWarning_WithHttpCodeAndWarnings_ShouldReturnWarningResponse()
        {
            var _warnings = new[] { "Partial data", "Limit applied" };
            var _response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result", _warnings);
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Message, Is.EqualTo("Partial result"));
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }
    }
}
