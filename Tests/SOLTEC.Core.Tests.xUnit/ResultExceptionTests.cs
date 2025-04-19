using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the ResultException class using xUnit.
/// </summary>
public class ResultExceptionTests
{
    /// <summary>
    /// Tests default constructor initializes an instance of ResultException.
    /// </summary>
    [Fact]
    public void Constructor_Default_InitializesInstance()
    {
        var _ex = new ResultException();

        Assert.NotNull(_ex);
    }

    /// <summary>
    /// Tests constructor with message and inner exception assigns values correctly.
    /// Sends custom message and inner exception and expects properties to match.
    /// </summary>
    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsProperties()
    {
        var _inner = new InvalidOperationException("inner");
        var _ex = new ResultException("custom message", _inner);

        Assert.Equal("custom message", _ex.Message);
        Assert.Equal(_inner, _ex.InnerException);
    }

    /// <summary>
    /// Tests setting all custom properties works as expected.
    /// </summary>
    [Fact]
    public void Properties_SetAndGet_WorkCorrectly()
    {
        var _ex = new ResultException
        {
            Key = "UserId",
            Reason = "User does not exist",
            ErrorMessage = "Not Found",
            HttpStatusCode = HttpStatusCode.NotFound
        };

        Assert.Equal("UserId", _ex.Key);
        Assert.Equal("User does not exist", _ex.Reason);
        Assert.Equal("Not Found", _ex.ErrorMessage);
        Assert.Equal(HttpStatusCode.NotFound, _ex.HttpStatusCode);
    }
}
