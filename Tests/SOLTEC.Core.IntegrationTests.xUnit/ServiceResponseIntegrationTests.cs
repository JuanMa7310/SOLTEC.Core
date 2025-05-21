using System.Net;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the non-generic ServiceResponse class covering all public static factory methods.
/// </summary>
public class ServiceResponseIntegrationTests
{
    [Fact]
    /// <summary>
    /// Tests CreateSuccess(int).
    /// </summary>
    public void CreateSuccess_WithIntCode_ShouldReturnValidResponse()
    {
        var _response = ServiceResponse.CreateSuccess(200);
        Assert.True(_response.Success);
        Assert.Equal(200, _response.ResponseCode);
        Assert.Null(_response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateSuccess(HttpStatusCode).
    /// </summary>
    public void CreateSuccess_WithHttpCode_ShouldReturnValidResponse()
    {
        var _response = ServiceResponse.CreateSuccess(HttpStatusCode.OK);
        Assert.True(_response.Success);
        Assert.Equal(200, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateSuccess with warnings.
    /// </summary>
    public void CreateSuccess_WithWarnings_ShouldReturnValidResponse()
    {
        var _warnings = new[] { "Delay notice" };
        var _response = ServiceResponse.CreateSuccess(200, _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError(int, message).
    /// </summary>
    public void CreateError_WithIntCode_ShouldReturnErrorResponse()
    {
        var _response = ServiceResponse.CreateError(500, "Internal Error");
        Assert.False(_response.Success);
        Assert.Equal(500, _response.ResponseCode);
        Assert.Equal("Internal Error", _response.ErrorMessage);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError(HttpStatusCode, message).
    /// </summary>
    public void CreateError_WithHttpCode_ShouldReturnErrorResponse()
    {
        var _response = ServiceResponse.CreateError(HttpStatusCode.InternalServerError, "Internal Error");
        Assert.False(_response.Success);
        Assert.Equal(500, _response.ResponseCode);
        Assert.Equal("Internal Error", _response.ErrorMessage);
    }

    [Fact]
    /// <summary>
    /// Tests CreateError with warnings.
    /// </summary>
    public void CreateError_WithWarnings_ShouldReturnErrorResponse()
    {
        var _warnings = new[] { "Deprecated API" };
        var _response = ServiceResponse.CreateError(500, "Internal Error", _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning(int, message).
    /// </summary>
    public void CreateWarning_WithIntCode_ShouldReturnWarningResponse()
    {
        var _response = ServiceResponse.CreateWarning(206, "Partial result");
        Assert.False(_response.Success);
        Assert.Equal("Partial result", _response.ErrorMessage);
        Assert.Equal(206, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning(HttpStatusCode, message).
    /// </summary>
    public void CreateWarning_WithHttpCode_ShouldReturnWarningResponse()
    {
        var _response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result");
        Assert.False(_response.Success);
        Assert.Equal("Partial result", _response.ErrorMessage);
        Assert.Equal(206, _response.ResponseCode);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning with warnings.
    /// </summary>
    public void CreateWarning_WithWarnings_ShouldReturnWarningResponse()
    {
        var _warnings = new[] { "Quota near limit" };
        var _response = ServiceResponse.CreateWarning(206, "Partial", _warnings);
        Assert.Equal(_warnings, _response.WarningMessages);
    }

    [Fact]
    /// <summary>
    /// Tests CreateWarning(HttpStatusCode, message, warnings).
    /// </summary>
    public void CreateWarning_WithHttpCodeAndWarnings_ShouldReturnWarningResponse()
    {
        var _warnings = new[] { "Partial data", "Limit applied" };
        var _response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result", _warnings);
        Assert.True(_response.Success);
        Assert.Equal("Partial result", _response.Message);
        Assert.Equal(_warnings, _response.WarningMessages);
    }
}
