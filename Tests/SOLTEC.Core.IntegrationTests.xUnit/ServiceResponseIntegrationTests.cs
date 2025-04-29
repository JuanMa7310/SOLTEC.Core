using Xunit;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class ServiceResponseIntegrationTests
    {
        [Fact]
        public void CreateSuccess_ShouldReturnSuccessTrue()
        {
            var _response = ServiceResponse.CreateSuccess("All good");
            Assert.True(_response.Success);
            Assert.Equal("All good", _response.Message);
        }

        [Fact]
        public void CreateError_ShouldReturnSuccessFalse()
        {
            var _response = ServiceResponse.CreateError("FAIL", "Something went wrong");
            Assert.False(_response.Success);
            Assert.Equal("FAIL", _response.Key);
            Assert.Equal("Something went wrong", _response.Message);
        }

        [Fact]
        public void CreateWarning_ShouldReturnPartialContent()
        {
            var _response = ServiceResponse.CreateWarning(206, "Warning message");
            Assert.True(_response.Success);
            Assert.Equal(206, _response.ResponseCode);
            Assert.Equal("Warning message", _response.Message);
        }
    }
}
