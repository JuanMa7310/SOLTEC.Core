using NUnit.Framework;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    /// <summary>
    /// Integration tests for the generic ServiceResponse&lt;T&gt; class covering public factory methods.
    /// </summary>
    public class ServiceResponseTIntegrationTests
    {
        [Test]
        /// <summary>
        /// Tests CreateSuccess(T data, int code)
        /// </summary>
        public void CreateSuccess_WithIntCode_ShouldReturnValidResponse()
        {
            var _data = "OK";
            var _response = ServiceResponse<string>.CreateSuccess(_data, 200);

            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Data, Is.EqualTo(_data));
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
        }

        [Test]
        /// <summary>
        /// Tests CreateSuccess(T data, HttpStatusCode code)
        /// </summary>
        public void CreateSuccess_WithHttpCode_ShouldReturnValidResponse()
        {
            var _data = "OK";
            var _response = ServiceResponse<string>.CreateSuccess(_data, HttpStatusCode.OK);

            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Data, Is.EqualTo(_data));
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
        }

        [Test]
        /// <summary>
        /// Tests CreateSuccess(T data, int code, string[]? warnings)
        /// </summary>
        public void CreateSuccess_WithWarnings_ShouldReturnValidResponse()
        {
            var _data = "OK";
            var _warnings = new[] { "Minor delay" };
            var _response = ServiceResponse<string>.CreateSuccess(_data, 200, _warnings);

            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(int, message)
        /// </summary>
        public void CreateError_WithCode_ShouldReturnErrorResponse()
        {
            var _response = ServiceResponse<string>.CreateError(400, "Bad request");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ErrorMessage, Is.EqualTo("Bad request"));
            Assert.That(_response.ResponseCode, Is.EqualTo(400));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(HttpStatusCode, message)
        /// </summary>
        public void CreateError_WithHttpCode_ShouldReturnErrorResponse()
        {
            var _response = ServiceResponse<string>.CreateError(HttpStatusCode.BadRequest, "Bad request");
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ErrorMessage, Is.EqualTo("Bad request"));
            Assert.That(_response.ResponseCode, Is.EqualTo(400));
        }

        [Test]
        /// <summary>
        /// Tests CreateError(int, message, warnings)
        /// </summary>
        public void CreateError_WithWarnings_ShouldReturnErrorResponse()
        {
            var _warnings = new[] { "Input missing" };
            var _response = ServiceResponse<string>.CreateError(400, "Bad request", _warnings);
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning with int code and message
        /// </summary>
        public void CreateWarning_WithIntCode_ShouldReturnWarningResponse()
        {
            var _response = ServiceResponse<string>.CreateWarning(206, "Partial result");
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Message, Is.EqualTo("Partial result"));
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning with HttpStatusCode and message
        /// </summary>
        public void CreateWarning_WithHttpCode_ShouldReturnWarningResponse()
        {
            var _response = ServiceResponse<string>.CreateWarning(HttpStatusCode.PartialContent, "Partial result");
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
            Assert.That(_response.Message, Is.EqualTo("Partial result"));
        }

        [Test]
        /// <summary>
        /// Tests CreateWarning with HttpStatusCode and warnings
        /// </summary>
        public void CreateWarning_WithWarnings_ShouldReturnWarningResponse()
        {
            var _warnings = new[] { "Rate limit applied" };
            var _response = ServiceResponse<string>.CreateWarning(HttpStatusCode.PartialContent, "Partial", _warnings);
            Assert.That(_response.WarningMessages, Is.EqualTo(_warnings));
        }
    }
}
