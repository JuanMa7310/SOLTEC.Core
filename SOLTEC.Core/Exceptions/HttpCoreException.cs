using System.Net;

namespace SOLTEC.Core.Exceptions;
public class HttpCoreException : ResultException
{
    public HttpCoreException(string key, string reason, HttpStatusCode httpStatusCode, string errorMessage = "")
    {
        Key = key;
        Reason = reason;
        ErrorMessage = errorMessage;
        HttpStatusCode = httpStatusCode;
    }
}

public enum HttpCoreErrorEnum
{
    BadRequest
}
