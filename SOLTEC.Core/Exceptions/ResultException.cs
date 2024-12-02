using System.Net;

namespace SOLTEC.Core.Exceptions;

public class ResultException : Exception
{
    public ResultException()
    {
        Key = string.Empty;
        Reason = string.Empty;
        ErrorMessage = string.Empty;
    }

    public ResultException(string message, Exception innerException) : base(message, innerException)
    {
        Key = string.Empty;
        Reason = string.Empty;
        ErrorMessage = string.Empty;
    }

    public string Key { get; set; }
    public string Reason { get; set; }
    public string ErrorMessage { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}
