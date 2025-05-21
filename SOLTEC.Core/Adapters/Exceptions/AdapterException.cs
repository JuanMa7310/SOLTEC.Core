using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using System.Net;

namespace SOLTEC.Core.Adapters.Exceptions;

/// <summary>
/// Exception thrown when an adapter operation fails with a specific error reason.
/// </summary>
/// <example>
/// <![CDATA[
/// throw new AdapterException(AdapterErrorEnum.ConnectionTimeout, "Failed to connect to data source.");
/// ]]>
/// </example>
public class AdapterException : ResultException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdapterException"/> class using the specified error reason, message, and HTTP status code.
    /// </summary>
    /// <param name="reason">The reason for the adapter failure.</param>
    /// <param name="errorMessage">The descriptive error message. Default is an empty string.</param>
    /// <param name="httpStatusCode">The associated HTTP status code. Default is BadRequest.</param>
    /// <example>
    /// <![CDATA[
    /// throw new AdapterException(AdapterErrorEnum.InvalidFormat, "Unsupported file structure.");
    /// ]]>
    /// </example>
    public AdapterException(AdapterErrorEnum reason, string errorMessage = "", HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        Key = "AdapterException";
        Reason = reason.ToString();
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
    }
}
