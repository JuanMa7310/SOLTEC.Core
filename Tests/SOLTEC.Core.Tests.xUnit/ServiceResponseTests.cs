using System.Net;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the ServiceResponse class using xUnit.
/// </summary>
public class ServiceResponseTests
{
    /// <summary>
    /// Tests that CreateSuccess with status code returns a successful response.
    /// Sends 200 and expects Success = true and ResponseCode = 200.
    /// </summary>
    [Fact]
    public void CreateSuccess_IntCode_ReturnsSuccessResponse()
    {
        var _response = ServiceResponse.CreateSuccess(200);

        Assert.True(_response.Success);
        Assert.Equal(200, _response.ResponseCode);
        Assert.Null(_response.ErrorMessage);
    }

    /// <summary>
    /// Tests that CreateError with error message returns an error response.
    /// Sends 500 and "Internal error" and expects Success = false and proper error message.
    /// </summary>
    [Fact]
    public void CreateError_IntCodeAndMessage_ReturnsErrorResponse()
    {
        var _response = ServiceResponse.CreateError(500, "Internal error");

        Assert.False(_response.Success);
        Assert.Equal(500, _response.ResponseCode);
        Assert.Equal("Internal error", _response.ErrorMessage);
    }

    /// <summary>
    /// Tests that CreateSuccess with warnings returns expected warning list.
    /// </summary>
    [Fact]
    public void CreateSuccess_WithWarnings_ReturnsWarnings()
    {
        var _warnings = new[] { "Low disk space" };
        var _response = ServiceResponse.CreateSuccess(200, _warnings);

        Assert.True(_response.Success);
        Assert.Contains("Low disk space", _response.WarningMessages!);
    }

    /// <summary>
    /// Tests that CreateError with warnings returns expected error and warnings.
    /// </summary>
    [Fact]
    public void CreateError_WithWarnings_ReturnsWarnings()
    {
        var _warnings = new[] { "Deprecated API" };
        var _response = ServiceResponse.CreateError(HttpStatusCode.BadRequest, "Bad input", _warnings);

        Assert.False(_response.Success);
        Assert.Equal((int)HttpStatusCode.BadRequest, _response.ResponseCode);
        Assert.Equal("Bad input", _response.ErrorMessage);
        Assert.Contains("Deprecated API", _response.WarningMessages!);
    }
}
