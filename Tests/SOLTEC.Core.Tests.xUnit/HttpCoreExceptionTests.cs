using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the HttpCoreException class using xUnit.
/// </summary>
public class HttpCoreExceptionTests
{
    [Fact]
    /// <summary>
    /// Verifies that all properties are correctly set when all parameters are provided.
    /// </summary>
    public void Constructor_WithAllParameters_AssignsCorrectValues()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.BadRequest, "Details", HttpCoreErrorEnum.BadRequest);

        Assert.Equal("Key", exception.Key);
        Assert.Equal("Reason", exception.Reason);
        Assert.Equal("Details", exception.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, exception.HttpStatusCode);
        Assert.Equal(HttpCoreErrorEnum.BadRequest, exception.ErrorType);
    }

    [Fact]
    /// <summary>
    /// Verifies that default values are assigned when parameters are null.
    /// </summary>
    public void Constructor_WithNulls_AssignsDefaultValues()
    {
        var exception = new HttpCoreException(null, null, HttpStatusCode.NotFound, null, null);

        Assert.Equal("Unknown Key", exception.Key);
        Assert.Equal("Unknown Reason", exception.Reason);
        Assert.Equal("", exception.ErrorMessage);
        Assert.Equal(HttpStatusCode.NotFound, exception.HttpStatusCode);
        Assert.Null(exception.ErrorType);
    }

    [Fact]
    /// <summary>
    /// Verifies that ErrorType reflects the correct enum when HttpStatusCode is known.
    /// </summary>
    public void Constructor_SetsCorrectErrorType()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.Unauthorized, "Unauthorized access", HttpCoreErrorEnum.Unauthorized);

        Assert.Equal(HttpCoreErrorEnum.Unauthorized, exception.ErrorType);
    }

    [Fact]
    /// <summary>
    /// Verifies that ErrorMessage can be an empty string and is respected.
    /// </summary>
    public void Constructor_AllowsEmptyErrorMessage()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.InternalServerError, "", HttpCoreErrorEnum.InternalServerError);

        Assert.Equal("", exception.ErrorMessage);
    }

    [Fact]
    /// <summary>
    /// Verifies that HttpCoreException inherits from ResultException.
    /// </summary>
    public void HttpCoreException_Is_ResultException()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.Conflict, "Conflict error", HttpCoreErrorEnum.Conflict);

        Assert.IsAssignableFrom<ResultException>(exception);
    }
}
