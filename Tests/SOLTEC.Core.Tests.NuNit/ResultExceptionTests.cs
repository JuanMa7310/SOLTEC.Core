﻿using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the ResultException class using NUnit.
/// </summary>
[TestFixture]
public class ResultExceptionNUnitTests
{
    /// <summary>
    /// Tests default constructor initializes an instance of ResultException.
    /// </summary>
    [Test]
    public void Constructor_Default_InitializesInstance()
    {
        var _ex = new ResultException();

        Assert.That(_ex, Is.Not.Null);
    }

    /// <summary>
    /// Tests constructor with message and inner exception assigns values correctly.
    /// Sends custom message and inner exception and expects properties to match.
    /// </summary>
    [Test]
    public void Constructor_WithMessageAndInnerException_SetsProperties()
    {
        var _inner = new InvalidOperationException("inner");
        var _ex = new ResultException("custom message", _inner);

        Assert.Multiple(() =>
        {
            Assert.That(_ex.Message, Is.EqualTo("custom message"));
            Assert.That(_ex.InnerException, Is.EqualTo(_inner));
        });
    }

    /// <summary>
    /// Tests setting all custom properties works as expected.
    /// </summary>
    [Test]
    public void Properties_SetAndGet_WorkCorrectly()
    {
        var _ex = new ResultException
        {
            Key = "UserId",
            Reason = "User does not exist",
            ErrorMessage = "Not Found",
            HttpStatusCode = HttpStatusCode.NotFound
        };

        Assert.Multiple(() =>
        {
            Assert.That(_ex.Key, Is.EqualTo("UserId"));
            Assert.That(_ex.Reason, Is.EqualTo("User does not exist"));
            Assert.That(_ex.ErrorMessage, Is.EqualTo("Not Found"));
            Assert.That(_ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }
}
