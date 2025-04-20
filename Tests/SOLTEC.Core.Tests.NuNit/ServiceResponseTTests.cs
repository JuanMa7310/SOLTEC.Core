using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the generic ServiceResponse&lt;T&gt; class using NUnit.
/// </summary>
public class ServiceResponseGenericTests
{
    /// <summary>
    /// Tests CreateSuccess with data and response code.
    /// Sends string data and 200, expects Success = true and correct Data.
    /// </summary>
    [Test]
    public void CreateSuccess_WithData_ReturnsSuccessResponse()
    {
        var _data = "Test message";
        var _response = ServiceResponse<string>.CreateSuccess(_data, 200);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
            Assert.That(_response.Data, Is.EqualTo(_data));
        });
    }

    /// <summary>
    /// Tests CreateError with code and message.
    /// Sends 400 and error text, expects Success = false and error message.
    /// </summary>
    [Test]
    public void CreateError_WithMessage_ReturnsErrorResponse()
    {
        var _error = "Validation failed";
        var _response = ServiceResponse<object>.CreateError(400, _error);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo(400));
            Assert.That(_response.ErrorMessage, Is.EqualTo(_error));
            Assert.That(_response.Data, Is.Null);
        });
    }

    /// <summary>
    /// Tests CreateSuccess with warnings.
    /// Sends data, code and warnings, expects warning array returned.
    /// </summary>
    [Test]
    public void CreateSuccess_WithWarnings_ReturnsWarningMessages()
    {
        var _warnings = new[] { "Deprecated endpoint" };
        var _response = ServiceResponse<string>.CreateSuccess("Ok", 200, _warnings);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.WarningMessages, Does.Contain("Deprecated endpoint"));
        });
    }

    /// <summary>
    /// Tests CreateError with HTTP status code and warnings.
    /// </summary>
    [Test]
    public void CreateError_WithHttpCodeAndWarnings_ReturnsWarningMessages()
    {
        var _warnings = new[] { "Check input" };
        var _response = ServiceResponse<int>.CreateError(HttpStatusCode.BadRequest, "Bad input", _warnings);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(_response.WarningMessages, Does.Contain("Check input"));
        });
    }
}