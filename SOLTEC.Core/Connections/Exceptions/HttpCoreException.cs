namespace SOLTEC.Core.Connections.Exceptions;

using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using System.Net;

/// <summary>
/// Represents an exception specific to HTTP core operations, extending <see cref="ResultException"/>.
/// </summary>
/// <example>
/// Example of throwing an HttpCoreException:
/// <![CDATA[
/// throw new HttpCoreException("InvalidInput", "Request body is not valid", HttpStatusCode.BadRequest, "Missing required fields");
/// ]]>
/// </example>
public class HttpCoreException : ResultException
{
    /// <summary>
    /// Indicates the categorized HTTP error type as an enum value.
    /// </summary>
    public HttpCoreErrorEnum? ErrorType { get; set; }

    /// <summary>
    /// Initializes a new instance of HttpCoreException with details.
    /// </summary>
    /// <param name="key">Key name for the error context.</param>
    /// <param name="reason">Technical reason for the error.</param>
    /// <param name="httpStatusCode">HTTP status code returned by the response.</param>
    /// <param name="errorMessage">Detailed error message (optional).</param>
    /// <param name="errorType">Mapped enum value from the status code (optional).</param>
    /// <example>
    /// <![CDATA[
    /// throw new HttpCoreException("Timeout", "The operation timed out", HttpStatusCode.RequestTimeout);
    /// ]]>
    /// </example>
    public HttpCoreException(string? key, string? reason, HttpStatusCode httpStatusCode, string? errorMessage = "", HttpCoreErrorEnum? errorType = null)
    {
        Key = key ?? "Unknown Key";
        Reason = reason ?? "Unknown Reason";
        ErrorMessage = errorMessage ?? "";
        HttpStatusCode = httpStatusCode;
        ErrorType = errorType;
    }
}
