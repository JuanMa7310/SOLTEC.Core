using Xunit;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.xUnit
{
    public class ServiceResponseGenericIntegrationTests
    {
        [Fact]
        public void CreateSuccess_ShouldReturnValidGenericResult()
        {
            var _response = ServiceResponse<string>.CreateSuccess("Value123", "Success!");

            Assert.True(_response.Success);
            Assert.Equal("Value123", _response.Result);
            Assert.Equal("Success!", _response.Message);
        }

        [Fact]
        public void CreateError_ShouldContainKeyMessageAndNullResult()
        {
            var _response = ServiceResponse<string>.CreateError("ERRCODE", "Fail occurred");

            Assert.False(_response.Success);
            Assert.Equal("ERRCODE", _response.Key);
            Assert.Equal("Fail occurred", _response.Message);
            Assert.Null(_response.Result);
        }

        [Fact]
        public void CreateWarning_ShouldYieldExpectedStructure()
        {
            var _response = ServiceResponse<string>.CreateWarning(206, "Partial content");

            Assert.True(_response.Success);
            Assert.Equal(206, _response.ResponseCode);
            Assert.Equal("Partial content", _response.Message);
            Assert.Null(_response.Result);
        }
    }
}
