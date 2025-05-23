namespace SOLTEC.Core.Connections.Exceptions;

using SOLTEC.Core.Exceptions;
using System.Net;

/// <summary>
/// Represents an exception specific to SOAP core operations, extending <see cref="ResultException"/>.
/// </summary>
/// <example>
/// <![CDATA[
/// using System.Net;
/// using SOLTEC.Core.Connections.Exceptions;
/// 
/// var _exception = new SoapCoreException(
///     "InvalidInput",
///     "Request body is not valid",
///     HttpStatusCode.BadRequest,
///     "Missing required fields"
/// );
/// throw exception;
/// ]]>
/// </example>
public class SoapCoreException : ResultException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoapCoreException"/> class with detailed information.
    /// </summary>
    /// <param name="key">Key name for the error context.</param>
    /// <param name="reason">Technical reason for the error.</param>
    /// <param name="httpStatusCode">HTTP status code associated with the error.</param>
    /// <param name="errorMessage">Optional detailed error message.</param>
    /// <example>
    /// <![CDATA[
    /// var _exception = new SoapCoreException(
    ///     "InvalidInput",
    ///     "Request body is not valid",
    ///     HttpStatusCode.BadRequest,
    ///     "Missing required fields"
    /// );
    /// throw exception;
    /// ]]>
    /// </example>
    public SoapCoreException(string key, string reason, HttpStatusCode httpStatusCode, string errorMessage = "")
    {
        Key = key;
        Reason = reason;
        ErrorMessage = errorMessage;
        HttpStatusCode = httpStatusCode;
    }
}
