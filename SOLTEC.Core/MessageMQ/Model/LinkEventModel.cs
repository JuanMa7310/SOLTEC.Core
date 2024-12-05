namespace SOLTEC.Core.MessageMQ.Model;

public class LinkEventModel 
{
    public Guid ParentId { get; private set; }
    public Guid EventId { get; private set; }
    public DateTime Creation { get; private set; }

    public LinkEventModel(Guid parentId, Guid eventId) 
    {
        ParentId = parentId;
        EventId = eventId;
        Creation = DateTime.Now;
    }

    public LinkEventModel(Guid parentId, Guid eventId, DateTime creation) 
    {
        ParentId = parentId;
        EventId = eventId;
        Creation = creation;
    }
}
