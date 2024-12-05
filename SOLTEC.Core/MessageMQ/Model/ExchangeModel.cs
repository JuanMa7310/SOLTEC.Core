using SOLTEC.Core.Extensions;

namespace SOLTEC.Core.MessageMQ.Model;

public class ExchangeModel 
{
    public int Id { get; }
    public Guid ExchangeId { get; }
    public Guid EventId { get; }
    public string AppName { get; }
    public string Message { get; }
    public string CreationDate { get; }
    public string ProcessedDate { get; }

    public ExchangeModel(Guid eventId, string appName, string message) 
    {
        DateTime? date = DateTime.Now;
        ExchangeId = Guid.NewGuid();
        EventId = eventId;
        AppName = appName;
        Message = message;
        CreationDate = date.ToDateFormatWithTime();
    }

    public ExchangeModel(Guid exchangeId, Guid eventId, string appName, string message, string processed, 
        string errorMessage) 
    {
        DateTime? date = DateTime.Now;
        ExchangeId = exchangeId;
        EventId = eventId;
        AppName = appName;
        Message = message;
        ProcessedDate = processed;
        CreationDate = date.ToDateFormatWithTime();
    }
}
