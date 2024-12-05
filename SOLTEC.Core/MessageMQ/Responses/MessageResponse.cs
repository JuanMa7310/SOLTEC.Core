namespace SOLTEC.Core.MessageMQ.Responses;

public class MessageResponse 
{
    public string Message { get; set; }
    public Guid MessageId { get; set; }
    public Guid EventMessageGuid { get; set; }
}
