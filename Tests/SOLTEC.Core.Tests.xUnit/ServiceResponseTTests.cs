using System.Net;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the generic ServiceResponse&lt;T&gt; class using xUnit.
/// </summary>
public class ServiceResponseGenericTests
{
    [Fact]
    /// <summary>
    /// Tests CreateSuccess with data and response code.
    /// Sends string data and 200, expects Success = true and correct Data.
    /// </summary>
    public void CreateSuccess_WithData_ReturnsSuccessResponse()
    {
        var _data = "Test message";
        var _response = ServiceResponse<string>.CreateSuccess(_data, 200);

        Assert.True(_response.Success);
        Assert.Equal(200, _response.ResponseCode);
        Assert.Equal(_data, _response.Data);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError with code and message.
    /// Sends 400 and error text, expects Success = false and error message.
    /// </summary>
    public void CreateError_WithMessage_ReturnsErrorResponse()
    {
        var _error = "Validation failed";
        var _response = ServiceResponse<object>.CreateError(400, _error);

        Assert.False(_response.Success);
        Assert.Equal(400, _response.ResponseCode);
        Assert.Equal(_error, _response.ErrorMessage);
        Assert.Null(_response.Data);
    }

    [Fact]
    /// <summary>
    /// Tests CreateSuccess with warnings.
    /// Sends data, code and warnings, expects warning array returned.
    /// </summary>
    public void CreateSuccess_WithWarnings_ReturnsWarningMessages()
    {
        var _warnings = new[] { "Deprecated endpoint" };
        var _response = ServiceResponse<string>.CreateSuccess("Ok", 200, _warnings);

        Assert.True(_response.Success);
        Assert.Contains("Deprecated endpoint", _response.WarningMessages!);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError with HTTP status code and warnings.
    /// </summary>
    public void CreateError_WithHttpCodeAndWarnings_ReturnsWarningMessages()
    {
        var _warnings = new[] { "Check input" };
        var _response = ServiceResponse<int>.CreateError(HttpStatusCode.BadRequest, "Bad input", _warnings);

        Assert.False(_response.Success);
        Assert.Equal((int)HttpStatusCode.BadRequest, _response.ResponseCode);
        Assert.Contains("Check input", _response.WarningMessages!);
    }
}