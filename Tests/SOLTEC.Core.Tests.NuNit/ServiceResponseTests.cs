﻿using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the ServiceResponse class using NUnit.
/// </summary>
[TestFixture]
public class ServiceResponseNUnitTests
{
    /// <summary>
    /// Tests that CreateSuccess with status code returns a successful response.
    /// Sends 200 and expects Success = true and ResponseCode = 200.
    /// </summary>
    [Test]
    public void CreateSuccess_IntCode_ReturnsSuccessResponse()
    {
        var _response = ServiceResponse.CreateSuccess(200);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.ResponseCode, Is.EqualTo(200));
            Assert.That(_response.ErrorMessage, Is.Null);
        });
    }

    /// <summary>
    /// Tests that CreateError with error message returns an error response.
    /// Sends 500 and "Internal error" and expects Success = false and proper error message.
    /// </summary>
    [Test]
    public void CreateError_IntCodeAndMessage_ReturnsErrorResponse()
    {
        var _response = ServiceResponse.CreateError(500, "Internal error");

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo(500));
            Assert.That(_response.ErrorMessage, Is.EqualTo("Internal error"));
        });
    }

    /// <summary>
    /// Tests that CreateSuccess with warnings returns expected warning list.
    /// </summary>
    [Test]
    public void CreateSuccess_WithWarnings_ReturnsWarnings()
    {
        var _warnings = new[] { "Low disk space" };
        var _response = ServiceResponse.CreateSuccess(200, _warnings);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.True);
            Assert.That(_response.WarningMessages, Does.Contain("Low disk space"));
        });
    }

    /// <summary>
    /// Tests that CreateError with warnings returns expected error and warnings.
    /// </summary>
    [Test]
    public void CreateError_WithWarnings_ReturnsWarnings()
    {
        var _warnings = new[] { "Deprecated API" };
        var _response = ServiceResponse.CreateError(HttpStatusCode.BadRequest, "Bad input", _warnings);

        Assert.Multiple(() =>
        {
            Assert.That(_response.Success, Is.False);
            Assert.That(_response.ResponseCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(_response.ErrorMessage, Is.EqualTo("Bad input"));
            Assert.That(_response.WarningMessages, Does.Contain("Deprecated API"));
        });
    }
}
