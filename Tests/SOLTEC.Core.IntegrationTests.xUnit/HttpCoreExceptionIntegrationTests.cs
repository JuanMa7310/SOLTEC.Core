using Xunit;
using SOLTEC.Core.Connections.Exceptions;
using System.Net;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class HttpCoreExceptionIntegrationTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            var _exception = new HttpCoreException("XKEY", "Forbidden", HttpStatusCode.Forbidden, "Not allowed", HttpCoreErrorEnum.Forbidden);

            Assert.Equal("XKEY", _exception.Key);
            Assert.Equal("Forbidden", _exception.Reason);
            Assert.Equal(HttpStatusCode.Forbidden, _exception.HttpStatusCode);
            Assert.Equal("Not allowed", _exception.ErrorMessage);
            Assert.Equal(HttpCoreErrorEnum.Forbidden, _exception.ErrorType);
        }
    }
}
