using SOLTEC.Core.Connections.Commands;
using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Xml.Serialization;

namespace SOLTEC.Core.Connections;

/// <summary>
/// Provides methods to send SOAP requests and handle SOAP responses.
/// </summary>
/// <example>
/// <![CDATA[
/// var parameters = new Dictionary<string, string>
/// {
///     { "Param1", "Value1" },
///     { "Param2", "Value2" }
/// };
/// 
/// var soapCommand = new SoapCommand(
///     "https://example.com/soap",
///     "ActionName",
///     "MethodName",
///     "http://example.com/namespace",
///     "username",
///     "password",
///     parameters
/// );
/// 
/// var soapCore = new SoapCore();
/// var response = await soapCore.Post<ResponseType>(soapCommand);
/// ]]>
/// </example>
/// <remarks>
/// Initializes a new instance of the <see cref="SoapCore"/> class with an optional custom HttpClient.
/// </remarks>
/// <param name="httpClient">Optional HttpClient instance. If not provided, a default one will be created.</param>
public class SoapCore(HttpClient? httpClient = null)
{
    private const string gcSoapEnvelopeNamespace = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string gcContentType = "text/xml";
    private const string gcSoapActionHeader = "SOAPAction";

    private readonly HttpClient gHttpClient = httpClient ?? CreateDefaultHttpClient();

    /// <summary>
    /// Sends a SOAP request using the provided command and returns the deserialized response.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized response.</typeparam>
    /// <param name="command">The SOAP command containing necessary request information.</param>
    /// <returns>The deserialized response object of type T.</returns>
    /// <example>
    /// <![CDATA[
    /// var soapCore = new SoapCore();
    /// var result = await soapCore.Post<ResponseType>(soapCommand);
    /// ]]>
    /// </example>
    public async Task<T> Post<T>(SoapCommand command)
    {
        var _soapEnvelope = CreateSoapEnvelope(command.soapMethod, command.soapNamespace, command.parameters);
        var _content = CreateContent(command.soapAction, _soapEnvelope);

        return await TryPost<T>(command.soapUrl, _content, command.username, command.password);
    }

    private static StringContent CreateContent(string action, string soapEnvelope)
    {
        var _content = new StringContent(soapEnvelope, Encoding.UTF8, gcContentType);
        AddSoapAction(action, _content);

        return _content;
    }

    private async Task<T> TryPost<T>(string url, StringContent content, string username, string password)
    {
        AuthenticationHeader(username, password, gHttpClient);

        try
        {
            var _response = await gHttpClient.PostAsync(url, content);
            _response.EnsureSuccessStatusCode();
            var _result = await _response.Content.ReadAsStringAsync();

            return DeserializeSoapResponse<T>(_result);
        }
        catch (Exception ex)
        {
            throw new HttpCoreException(
                nameof(HttpCoreException),
                nameof(SoapCoreErrorEnum.BadRequest),
                (HttpStatusCode)SoapCoreErrorEnum.BadRequest,
                ex.Message
            );
        }
    }

    private static void AddSoapAction(string action, StringContent content)
    {
        if (string.IsNullOrEmpty(action)) return;
        content.Headers.Add(gcSoapActionHeader, action);
    }

    private static string CreateSoapEnvelope(string soapMethod, string soapNamespace, Dictionary<string, string> parameters)
    {
        var _soapBody = new StringBuilder();

        _soapBody.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        _soapBody.AppendFormat("<soap:Envelope xmlns:soap=\"{0}\">", gcSoapEnvelopeNamespace);
        _soapBody.Append("<soap:Body>");
        _soapBody.AppendFormat("<{0} xmlns=\"{1}\">", soapMethod, soapNamespace);

        foreach (var _param in parameters)
        {
            _soapBody.AppendFormat("<{0}>{1}</{0}>", _param.Key, _param.Value);
        }

        _soapBody.AppendFormat("</{0}>", soapMethod);
        _soapBody.Append("</soap:Body>");
        _soapBody.Append("</soap:Envelope>");

        return _soapBody.ToString();
    }

    private T DeserializeSoapResponse<T>(string soapResponse)
    {
        try
        {
            var _startIndex = soapResponse.IndexOf("<soap:Body>") + "<soap:Body>".Length;
            var _endIndex = soapResponse.IndexOf("</soap:Body>");
            var _bodyContent = soapResponse[_startIndex.._endIndex];
            using var _stringReader = new StringReader(_bodyContent);
            var _serializer = new XmlSerializer(typeof(T));
            var _deserializedObject = _serializer.Deserialize(_stringReader);

            return _deserializedObject is null
                ? throw new HttpCoreException(
                    nameof(HttpCoreException),
                    nameof(SoapCoreErrorEnum.Deserialization),
                    (HttpStatusCode)SoapCoreErrorEnum.BadRequest,
                    "SOAP response deserialization returned null."
                )
                : (T)_deserializedObject;
        }
        catch (Exception ex)
        {
            throw new HttpCoreException(
                nameof(HttpCoreException),
                nameof(SoapCoreErrorEnum.Deserialization),
                (HttpStatusCode)SoapCoreErrorEnum.BadRequest,
                ex.Message
            );
        }
    }

    private static void AuthenticationHeader(string username, string password, HttpClient httpClient)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) 
            return;

        var _byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(_byteArray));
    }

    private static HttpClient CreateDefaultHttpClient()
    {
        var _clientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
            SslProtocols = SslProtocols.Tls12
        };

        return new HttpClient(_clientHandler);
    }

    private static void AS()
    {
        var _httpClient = CreateDefaultHttpClient();
        _httpClient.GetAsync("http://www.googlee.es");
    }
}