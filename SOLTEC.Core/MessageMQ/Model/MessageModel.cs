using SOLTEC.Core.Extensions;
using System.Net;

namespace SOLTEC.Core.MessageMQ.Model;

public class MessageModel {
    public Guid MessageGuid { get; private set; }
    public Guid EventMessageGuid { get; private set; }
    public string EventName { get; private set; }
    public string SubscriberName { get; private set; }
    public string Message { get; private set; }
    public string CreationDate { get; private set; }
    public string ProcessedDate { get; private set; }
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public MessageModel(Guid eventMessageGuid, string eventName, string subscriberName, string message) 
    {
        DateTime? date = DateTime.Now;
        MessageGuid = Guid.NewGuid();
        EventMessageGuid = eventMessageGuid;
        EventName = eventName;
        SubscriberName = subscriberName;
        Message = message;
        CreationDate = date.ToDateFormatWithTime();
        ProcessedDate = string.Empty;
        StatusCode = HttpStatusCode.OK;
        ErrorMessage = string.Empty;
    }

    public MessageModel(Guid messageGuid, Guid eventMessageGuid, string eventName, string subscriberName, 
        string message, string creationDate, string processedDate, int statusCode, string errorMessage) 
    {
        MessageGuid = messageGuid;
        EventMessageGuid = eventMessageGuid;
        EventName = eventName;
        SubscriberName = subscriberName;
        Message = message;
        CreationDate = creationDate;
        ProcessedDate = processedDate;
        StatusCode = (HttpStatusCode)statusCode;
        ErrorMessage = errorMessage;
    }

    public void UpdateProcessed() 
    {
        DateTime? date = DateTime.Now;
        ProcessedDate = date.ToDateFormatWithTime();
    }

    public void UpdateStatusCode(HttpStatusCode statusCode, string errorMessage) 
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}
