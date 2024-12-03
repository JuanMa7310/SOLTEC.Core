namespace SOLTEC.Core.Connections.Commands;

public record SoapCommand(string soapUrl, string soapAction, string soapMethod, 
    string soapNamespace, string username, string password,  Dictionary<string, string> parameters);
