using System.Net;
using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Exceptions;

public class MessageMQException : ResultException
{
    public MessageMQException(EventMessageErrorEnum reason, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, 
        string errorMessage = "")
    {
        Key = "MessageMQException";
        HttpStatusCode = httpStatusCode;
        Reason = reason.ToString();
        ErrorMessage = errorMessage;
    }
}

