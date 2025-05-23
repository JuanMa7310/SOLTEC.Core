namespace SOLTEC.Core.Enums;

/// <summary>
/// Enumerates common HTTP error codes for structured exception handling in HttpCore.
/// </summary>
/// <example>
/// Example usage:
/// <![CDATA[
/// HttpCoreErrorEnum _httpError = HttpCoreErrorEnum.Json;
/// if (_httpError == HttpCoreErrorEnum.BadRequest)
/// {
///     Console.WriteLine("Http protocol response with Error (BaddReques) EndPoint not found.");
/// }
/// ]]>
/// </example>
public enum HttpCoreErrorEnum
{
    /// <summary>
    /// The server could not understand the request due to invalid syntax (400).
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Authentication is required and has failed or has not been provided (401).
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// The client does not have access rights to the content (403).
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// The requested resource could not be found on the server (404).
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// The request method is known by the server but is not supported by the target resource (405).
    /// </summary>
    MethodNotAllowed = 405,

    /// <summary>
    /// The request conflicts with the current state of the server (409).
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// The server encountered an unexpected condition that prevented it from fulfilling the request (500).
    /// </summary>
    InternalServerError = 500,

    /// <summary>
    /// The request method is not supported by the server and cannot be handled (501).
    /// </summary>
    NotImplemented = 501,

    /// <summary>
    /// The server received an invalid response from the upstream server (502).
    /// </summary>
    BadGateway = 502,

    /// <summary>
    /// The server is not ready to handle the request (503).
    /// </summary>
    ServiceUnavailable = 503,

    /// <summary>
    /// The server, while acting as a gateway, did not receive a timely response (504).
    /// </summary>
    GatewayTimeout = 504
}