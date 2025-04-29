namespace SOLTEC.Core.Enums;

/// <summary>
/// Enumerates specific error types for SOAP core operations.
/// </summary>
/// <example>
/// Example of using SoapCoreErrorEnum:
/// <![CDATA[
/// if (response.StatusCode == HttpStatusCode.BadRequest)
/// {
///     throw new SoapCoreException(nameof(SoapCoreErrorEnum.BadRequest), "Invalid SOAP request", HttpStatusCode.BadRequest);
/// }
/// ]]>
/// </example>
public enum SoapCoreErrorEnum
{
    /// <summary>
    /// Indicates that the SOAP request was invalid or malformed.
    /// Typically maps to an HTTP 400 Bad Request status.
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Indicates a failure during the deserialization of a SOAP response.
    /// Typically used when parsing XML fails or the response structure is incorrect.
    /// </summary>
    Deserialization = 401
}
