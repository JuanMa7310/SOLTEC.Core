using Xunit;
using SOLTEC.Core;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class ResultExceptionIntegrationTests
    {
        [Fact]
        public void Constructor_WithArguments_ShouldSetProperties()
        {
            var _exception = new ResultException("KEY42", "Logic error", HttpStatusCode.InternalServerError, "Stack trace hidden");

            Assert.Equal("KEY42", _exception.Key);
            Assert.Equal("Logic error", _exception.Reason);
            Assert.Equal(HttpStatusCode.InternalServerError, _exception.HttpStatusCode);
            Assert.Equal("Stack trace hidden", _exception.ErrorMessage);
        }
    }
}
