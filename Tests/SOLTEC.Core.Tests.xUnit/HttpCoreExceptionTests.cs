using SOLTEC.Core.Connections.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the HttpCoreException class using xUnit.
/// </summary>
public class HttpCoreExceptionTests
{
    /// <summary>
    /// Tests constructor assigns all properties correctly.
    /// Sends values for key, reason, status code, and message. Expects properties to match.
    /// </summary>
    [Fact]
    public void Constructor_AssignsAllProperties_Correctly()
    {
        var _exception = new HttpCoreException("InvalidInput", "Missing fields", HttpStatusCode.BadRequest, "Fields are required");

        Assert.Equal("InvalidInput", _exception.Key);
        Assert.Equal("Missing fields", _exception.Reason);
        Assert.Equal("Fields are required", _exception.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, _exception.HttpStatusCode);
    }

    /// <summary>
    /// Tests default value of errorMessage when not provided.
    /// Sends key, reason, and status code. Expects empty ErrorMessage.
    /// </summary>
    [Fact]
    public void Constructor_WithoutErrorMessage_SetsEmptyString()
    {
        var _exception = new HttpCoreException("InvalidInput", "Missing fields", HttpStatusCode.BadRequest);

        Assert.Equal(string.Empty, _exception.ErrorMessage);
    }
}