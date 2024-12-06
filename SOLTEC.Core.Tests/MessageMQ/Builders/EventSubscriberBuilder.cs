using SOLTEC.Core.DataSources.Interfaces;

namespace SOLTEC.Core.Tests.MessageMQ.Builders;

public class EventSubscriberBuilder 
{
    private readonly IDataSource _dataSource;
    private Guid _eventId = DefaultValues.EventId;
    private Guid _subscriptorId = DefaultValues.SubscriberGuid;
    private string _eventName = DefaultValues.EventName;
    private readonly Guid _eventSubscriptorId = DefaultValues.EventSubscriptorId;
    private readonly string _creation = DefaultValues.CreationWithTime;
    private readonly bool _active = true;
    private EventMessageBuilder _eventBuilder;
    private SubscriberBuilder _subscriptorBuilder;

    public EventSubscriberBuilder(IDataSource dataSource) 
    {
        _dataSource = dataSource;
        _eventBuilder = new EventMessageBuilder(_dataSource);
        _subscriptorBuilder = new SubscriberBuilder(_dataSource);
    }

    public EventSubscriberBuilder WithEventId(Guid value) 
    {
        _eventId = value;
        
        return this;
    }

    public EventSubscriberBuilder WithEventName(string value) 
    {
        _eventName = value;
        
        return this;
    }

    public void Build() 
    {
        _eventBuilder.WithEventMessageGuid(_eventId)
                    .WithEventName(_eventName)
                    .Build();
        _subscriptorBuilder.Build();
        _dataSource.ExecuteDapper<object>(@"
            if (select count(0) from SOLTECMQ.dbo.tbEventSubscriptor where EventId = @eventId and SubscriptorId = @subscriptorId) = 0
            begin
                insert into dbo.tbEventSubscriptor (                        
                    RowId,
                    EventId,
                    SubscriptorId, 
                    Creation,
                    Active) 
                values (                        
                    @rowId, 
                    @eventId,
                    @subscriptorId, 
                    @creation, 
                    @active)
            end", 
            new {
                rowId = _eventSubscriptorId,
                eventId = _eventId,
                subscriptorId = _subscriptorId,
                creation = _creation,
                active = _active
            });
    }

    public EventSubscriberBuilder WithSubscriptorId(Guid value) 
    {
        _subscriptorId = value;
        
        return this;
    }
}
