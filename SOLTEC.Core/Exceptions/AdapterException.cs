using System.Net;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;

namespace SOLTEC.Core.Adapters.Exceptions;

public class AdapterException : ResultException
{
    public AdapterException(AdapterErrorEnum reason, string errorMessage = "", 
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        Key = "AdapterException";
        Reason = reason.ToString();
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
    }
}
