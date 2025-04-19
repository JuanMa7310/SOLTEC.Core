using SOLTEC.Core.Connections.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the HttpCoreException class using NUnit.
/// </summary>
[TestFixture]
public class HttpCoreExceptionTests
{
    /// <summary>
    /// Tests constructor assigns all properties correctly.
    /// Sends values for key, reason, status code, and message. Expects properties to match.
    /// </summary>
    [Test]
    public void Constructor_AssignsAllProperties_Correctly()
    {
        var _exception = new HttpCoreException("InvalidInput", "Missing fields", HttpStatusCode.BadRequest, "Fields are required");

        Assert.Multiple(() =>
        {
            Assert.That(_exception.Key, Is.EqualTo("InvalidInput"));
            Assert.That(_exception.Reason, Is.EqualTo("Missing fields"));
            Assert.That(_exception.ErrorMessage, Is.EqualTo("Fields are required"));
            Assert.That(_exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        });
    }

    /// <summary>
    /// Tests default value of errorMessage when not provided.
    /// Sends key, reason, and status code. Expects empty ErrorMessage.
    /// </summary>
    [Test]
    public void Constructor_WithoutErrorMessage_SetsEmptyString()
    {
        var _exception = new HttpCoreException("InvalidInput", "Missing fields", HttpStatusCode.BadRequest);

        Assert.That(_exception.ErrorMessage, Is.EqualTo(string.Empty));
    }
}
