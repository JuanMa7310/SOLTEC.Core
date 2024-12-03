using System.Net;
using SOLTEC.Core.Exceptions;

namespace SOLTEC.Core.Connections.Exceptions
{
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
}
