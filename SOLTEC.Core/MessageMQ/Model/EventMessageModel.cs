using SOLTEC.Core.Enums;
using SOLTEC.Core.Extensions;
using SOLTEC.Core.Exceptions;

namespace SOLTEC.MQApiBusiness.EventMessage.Models;

public class EventMessageModel 
{
    public Guid EventMessageGuid { get; }
    public string Publisher { get; }
    public string EventName { get; private set; }
    public string CreationDate { get; }
    public string Message { get; }
    public string ProcessedDate { get; private set; }        


    public EventMessageModel(string publisher, string eventName, string message) 
    {
        IsValidEventName(eventName);
        DateTime? dateTime = DateTime.UtcNow;
        EventMessageGuid = Guid.NewGuid();
        Publisher = publisher;
        EventName = eventName;
        Message = message;
        CreationDate = dateTime.ToDateFormatWithTime();
        ProcessedDate = string.Empty;
    }

    public EventMessageModel(Guid eventMessageGuid, string publisher, string eventName, string creationDate, 
        string message, string processedDate) 
    {
        EventMessageGuid = eventMessageGuid;
        Publisher = publisher;
        EventName = eventName;
        CreationDate = creationDate;
        Message = message;
        ProcessedDate = processedDate;
    }

    private void IsValidEventName(string eventName) 
    {
        eventName = eventName.Trim();
        if (string.IsNullOrEmpty(eventName) || eventName.Contains(' ')) 
            throw new MessageMQException(EventMessageErrorEnum.InvalidEventName);
    }

    public void UpdateToProcessed() 
    {
        DateTime dateTime = DateTime.UtcNow;
        ProcessedDate = dateTime.ToDateFormatWithTime();
    }
}
