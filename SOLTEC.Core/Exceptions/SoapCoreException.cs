using System.Net;

namespace SOLTEC.Core.Exceptions;

public class SoapCoreException : ResultException 
{
    public SoapCoreException(string key, string reason, HttpStatusCode httpStatusCode, string errorMessage = "") {
        Key = key;
        Reason = reason;
        ErrorMessage = errorMessage;
        HttpStatusCode = httpStatusCode;
    }
}

public enum SoapCoreErrorEnum 
{
    BadRequest = 400,
    Deserialization = 401
}
