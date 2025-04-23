using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the generic ServiceResponse&lt;T&gt; class using NUnit.
/// </summary>
public class ServiceResponseGenericTests
{
    [Test]
    /// <summary>
    /// Tests CreateSuccess with data and response code.
    /// Sends string data and 200, expects Success = true and correct Data.
    /// </summary>
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

    [Test]
    /// <summary>
    /// Tests CreateError with code and message.
    /// Sends 400 and error text, expects Success = false and error message.
    /// </summary>
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

    [Test]
    /// <summary>
    /// Tests CreateSuccess with warnings.
    /// Sends data, code and warnings, expects warning array returned.
    /// </summary>
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

    [Test]
    /// <summary>
    /// Tests CreateError with HTTP status code and warnings.
    /// </summary>
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

    [Test]
    /// <summary>
    /// Verifies that CreateWarning creates a ServiceResponseT with Success = true,
    /// correct message, response code, and a null Result.
    /// </summary>
    public void CreateWarning_ShouldReturnValidWarningResponseT()
    {
        var _response = ServiceResponse<string>.CreateWarning(206, "Warning message");

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.Message, Is.EqualTo("Warning message"));
            Assert.That(_response.ResponseCode, Is.EqualTo(206));
            Assert.That(_response.Result, Is.Null);
        });
    }
}