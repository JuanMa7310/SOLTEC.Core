using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the HttpCoreException class using NUnit.
/// </summary>
public class HttpCoreExceptionTests
{
    [Test]
    /// <summary>
    /// Verifies that all properties are correctly set when all parameters are provided.
    /// </summary>
    public void Constructor_WithAllParameters_AssignsCorrectValues()
    {
        var exception = new HttpCoreException(
            key: "InvalidInput",
            reason: "The input format is not correct",
            httpStatusCode: HttpStatusCode.BadRequest,
            errorMessage: "The field 'name' is required",
            errorType: HttpCoreErrorEnum.BadRequest);

        Assert.Multiple(() =>
        {
            Assert.That(exception.Key, Is.EqualTo("InvalidInput"));
            Assert.That(exception.Reason, Is.EqualTo("The input format is not correct"));
            Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.ErrorMessage, Is.EqualTo("The field 'name' is required"));
            Assert.That(exception.ErrorType, Is.EqualTo(HttpCoreErrorEnum.BadRequest));
        });
    }

    [Test]
    /// <summary>
    /// Verifies that default values are assigned when parameters are null.
    /// </summary>
    public void Constructor_WithNulls_AssignsDefaultValues()
    {
        var exception = new HttpCoreException(null, null, HttpStatusCode.NotFound);

        Assert.Multiple(() =>
        {
            Assert.That(exception.Key, Is.EqualTo("Unknown Key"));
            Assert.That(exception.Reason, Is.EqualTo("Unknown Reason"));
            Assert.That(exception.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(exception.ErrorMessage, Is.EqualTo(""));
            Assert.That(exception.ErrorType, Is.Null);
        });
    }

    [Test]
    /// <summary>
    /// Verifies that ErrorType reflects the correct enum when HttpStatusCode is known.
    /// </summary>
    public void Constructor_SetsCorrectErrorType()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.Unauthorized, "Unauthorized access", HttpCoreErrorEnum.Unauthorized);

        Assert.That(exception.ErrorType, Is.EqualTo(HttpCoreErrorEnum.Unauthorized));
    }

    [Test]
    /// <summary>
    /// Verifies that ErrorMessage can be an empty string and is respected.
    /// </summary>
    public void Constructor_AllowsEmptyErrorMessage()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.InternalServerError, "", HttpCoreErrorEnum.InternalServerError);

        Assert.That(exception.ErrorMessage, Is.EqualTo(""));
    }

    [Test]
    /// <summary>
    /// Verifies that HttpCoreException inherits from ResultException.
    /// </summary>
    public void HttpCoreException_Is_ResultException()
    {
        var exception = new HttpCoreException("Key", "Reason", HttpStatusCode.Conflict, "Conflict error", HttpCoreErrorEnum.Conflict);

        Assert.That(exception, Is.InstanceOf<ResultException>());
    }
}
