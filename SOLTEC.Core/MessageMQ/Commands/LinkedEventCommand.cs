namespace SOLTEC.Core.MessageMQ.Commands;

public class LinkedEventCommand 
{
    public Guid ParentId { get; set; }
    public Guid EventId { get; set; }
}
