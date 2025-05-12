using System.Net;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the generic ServiceResponse&lt;T&gt; class covering all public static methods.
/// </summary>
public class ServiceResponseTIntegrationTests
{
    [Fact]
    /// <summary>
    /// Tests CreateSuccess(T data, int code)
    /// Sends a string and expects a success response with code 200.
    /// </summary>
    public void CreateSuccess_WithIntCode_ShouldReturnValidResponse()
    {
        var _data = "OK";
        var _response = ServiceResponse<string>.CreateSuccess(_data, 200);
        Assert.True(_response.Success);
        Assert.Equal(_data, _response.Data);
        Assert.Equal(200, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateSuccess(T data, HttpStatusCode code)
    /// </summary>
    public void CreateSuccess_WithHttpCode_ShouldReturnValidResponse()
    {
        var _data = "OK";
        var _response = ServiceResponse<string>.CreateSuccess(_data, HttpStatusCode.OK);
        Assert.True(_response.Success);
        Assert.Equal(_data, _response.Data);
        Assert.Equal(200, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateSuccess(T data, int code, string[]? warnings)
    /// </summary>
    public void CreateSuccess_WithWarnings_ShouldReturnValidResponse()
    {
        var _data = "OK";
        var _warnings = new[] { "Minor delay" };
        var _response = ServiceResponse<string>.CreateSuccess(_data, 200, _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError(int, message)
    /// </summary>
    public void CreateError_WithCode_ShouldReturnErrorResponse()
    {
        var _response = ServiceResponse<string>.CreateError(400, "Bad request");
        Assert.False(_response.Success);
        Assert.Equal("Bad request", _response.ErrorMessage);
        Assert.Equal(400, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError(HttpStatusCode, message)
    /// </summary>
    public void CreateError_WithHttpCode_ShouldReturnErrorResponse()
    {
        var _response = ServiceResponse<string>.CreateError(HttpStatusCode.BadRequest, "Bad request");
        Assert.False(_response.Success);
        Assert.Equal("Bad request", _response.ErrorMessage);
        Assert.Equal(400, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError(int, message, warnings)
    /// </summary>
    public void CreateError_WithWarnings_ShouldReturnErrorResponse()
    {
        var _warnings = new[] { "Input missing" };
        var _response = ServiceResponse<string>.CreateError(400, "Bad request", _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning with int code and message
    /// </summary>
    public void CreateWarning_WithIntCode_ShouldReturnWarningResponse()
    {
        var _response = ServiceResponse<string>.CreateWarning(206, "Partial result");
        Assert.True(_response.Success);
        Assert.Equal("Partial result", _response.Message);
        Assert.Equal(206, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning with HttpStatusCode and message
    /// </summary>
    public void CreateWarning_WithHttpCode_ShouldReturnWarningResponse()
    {
        var _response = ServiceResponse<string>.CreateWarning(HttpStatusCode.PartialContent, "Partial result");
        Assert.True(_response.Success);
        Assert.Equal(206, _response.ResponseCode);
        Assert.Equal("Partial result", _response.Message);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning with HttpStatusCode and warnings
    /// </summary>
    public void CreateWarning_WithWarnings_ShouldReturnWarningResponse()
    {
        var _warnings = new[] { "Rate limit applied" };
        var _response = ServiceResponse<string>.CreateWarning(HttpStatusCode.PartialContent, "Partial", _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }
}
