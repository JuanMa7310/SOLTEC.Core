namespace SOLTEC.Core.Connections.Commands;

/// <summary>
/// Represents a SOAP command with all necessary information to perform a SOAP request.
/// </summary>
/// <param name="soapUrl">The URL endpoint of the SOAP service.</param>
/// <param name="soapAction">The action or operation to be performed on the SOAP service.</param>
/// <param name="soapMethod">The specific method to invoke in the SOAP service.</param>
/// <param name="soapNamespace">The namespace used in the SOAP envelope.</param>
/// <param name="username">The username used for authentication, if required.</param>
/// <param name="password">The password used for authentication, if required.</param>
/// <param name="parameters">A dictionary containing the parameter names and values to be sent in the SOAP request.</param>
/// <example>
/// <![CDATA[
/// // Example of how to create and use a SoapCommand:
/// var _parameters = new Dictionary<string, string>
/// {
///     { "Parameter1", "Value1" },
///     { "Parameter2", "Value2" }
/// };
/// 
/// var _soapCommand = new SoapCommand(
///     "https://example.com/soap",
///     "ActionName",
///     "MethodName",
///     "http://example.com/namespace",
///     "username",
///     "password",
///     _parameters
/// );
/// ]]>
/// </example>
public record SoapCommand(string soapUrl, string soapAction, string soapMethod, string soapNamespace, string username, string password, Dictionary<string, string> parameters);
