using SOLTEC.Core.Connections.Commands;
using SOLTEC.Core.Connections.Exceptions;
using MoreLinq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Xml.Serialization;

namespace SOLTEC.Core.Connections;

public class SoapCore 
{
    public async Task<T?> Post<T>(SoapCommand command) 
    {
        var soapEnvelope = CreateSoapEnvelope(command.soapMethod, command.soapNamespace, command.parameters);
        var content = CreateContent(command.soapAction, soapEnvelope);
        
        return await TryPost<T>(command.soapUrl, content, command.username, command.password);
    }

    private StringContent CreateContent(string action, string soapEnvelope) 
    {
        var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        AddSoapAction(action, content);
    
        return content;
    }

    private async Task<T?> TryPost<T>(string url, StringContent content, string username, string password) 
    {
        using HttpClient httpClient = new(CreateHttpClientHandler());
        AuthenticationHeader(username, password, httpClient);
        
        try 
        {
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return DeserializeSoapResponse<T>(result);
        }
        catch (Exception ex) 
        {
            throw new HttpCoreException(nameof(HttpCoreException), nameof(SoapCoreErrorEnum.BadRequest), 
                (HttpStatusCode)SoapCoreErrorEnum.BadRequest, ex.Message);
        }
    }

    private void AddSoapAction(string action, StringContent content) 
    {
        if (string.IsNullOrEmpty(action)) 
            return;
        content.Headers.Add("SOAPAction", action);
    }

    private string CreateSoapEnvelope(string soapMethod, string soapNamespace, Dictionary<string, string> parameters) 
    {
        var soapBody = new StringBuilder();
        
        soapBody.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        soapBody.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
        soapBody.Append("<soap:Body>");
        soapBody.AppendFormat("<{0} xmlns=\"{1}\">", soapMethod, soapNamespace);
        parameters.ForEach(p => soapBody.AppendFormat("<{0}>{1}</{0}>", p.Key, p.Value));
        soapBody.AppendFormat("</{0}>", soapMethod);
        soapBody.Append("</soap:Body>");
        soapBody.Append("</soap:Envelope>");
        
        return soapBody.ToString();
    }

    private T? DeserializeSoapResponse<T>(string soapResponse) 
    {
        try 
        {
            var startIndex = soapResponse.IndexOf("<soap:Body>", StringComparison.Ordinal) + "<soap:Body>".Length;
            var endIndex = soapResponse.IndexOf("</soap:Body>", StringComparison.Ordinal);
            var bodyContent = soapResponse.Substring(startIndex, endIndex - startIndex);
            
            using (var stringReader = new StringReader(bodyContent)) 
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader)!;
            }
        }
        catch (Exception ex) 
        {
            throw new HttpCoreException(nameof(HttpCoreException), 
                nameof(SoapCoreErrorEnum.Deserialization), 
                (HttpStatusCode)SoapCoreErrorEnum.BadRequest, 
                ex.Message);
        }
    }

    private static void AuthenticationHeader(string username, string password, HttpClient httpClient) 
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) 
            return;
        var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
            Convert.ToBase64String(byteArray));
    }

    private static HttpClientHandler CreateHttpClientHandler() 
    {
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, 
            chain, sslPolicyErrors) => { return true; };
        clientHandler.SslProtocols |= SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
        
        return clientHandler;
    }
}
