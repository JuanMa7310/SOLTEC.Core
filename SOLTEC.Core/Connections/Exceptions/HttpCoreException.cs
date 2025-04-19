using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Connections.Exceptions;

/// <summary>
/// Represents an exception specific to HTTP core operations, extending <see cref="ResultException"/>.
/// </summary>
/// <example>
/// Example of throwing an HttpCoreException:
/// <code>
/// throw new HttpCoreException("InvalidInput", "Request body is not valid", HttpStatusCode.BadRequest, "Missing required fields");
/// </code>
/// </example>
public class HttpCoreException : ResultException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpCoreException"/> class with specified details.
    /// </summary>
    /// <param name="key">The key identifying the error source.</param>
    /// <param name="reason">A short description of the reason.</param>
    /// <param name="httpStatusCode">The HTTP status code associated with the error.</param>
    /// <param name="errorMessage">The detailed error message (optional).</param>
    public HttpCoreException(string key, string reason, HttpStatusCode httpStatusCode, string errorMessage = "")
    {
        Key = key;
        Reason = reason;
        ErrorMessage = errorMessage;
        HttpStatusCode = httpStatusCode;
    }
}
