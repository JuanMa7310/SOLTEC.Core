using System.Net;

namespace SOLTEC.Core.Exceptions;

/// <summary>
/// Represents a custom exception that includes additional context such as a key, reason, error message, and HTTP status code.
/// </summary>
/// <example>
/// Example of throwing a ResultException:
/// <![CDATA[
/// throw new ResultException("Resource not found", new KeyNotFoundException())
/// {
///     Key = "UserId",
///     Reason = "User does not exist",
///     ErrorMessage = "Unable to locate user by given ID.",
///     HttpStatusCode = HttpStatusCode.NotFound
/// };
/// ]]>
/// </example>
public class ResultException : Exception
{
    /// <summary>
    /// Gets or sets a key that identifies the cause or context of the exception.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets a short reason description.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Gets or sets the detailed error message.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the associated HTTP status code.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultException"/> class.
    /// </summary>
    public ResultException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultException"/> class with a specified message and inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ResultException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
