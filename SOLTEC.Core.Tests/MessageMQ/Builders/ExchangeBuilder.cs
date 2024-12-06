using SOLTEC.Core.DataSources.Interfaces;

namespace SOLTEC.Core.Tests.MessageMQ.Builders;

public class ExchangeBuilder 
{
    private readonly IDataSource _dataSource;
    private Guid _exchangeGuid = DefaultValues.ExchangeId;
    private string _eventName = DefaultValues.EventName;
    private string _subscriberName = DefaultValues.SubscriberName;
    private bool _active = true;
    private readonly string _creationDate = DefaultValues.CreationWithTime;

    public ExchangeBuilder(IDataSource dataSource) 
    {
        _dataSource = dataSource;
    }

    public void Build() 
    {
        _dataSource.Execute(@"insert into MQ.Exchange ( ExchangeGuid, EventName, SubscriberName, CreationDate, Active) 
            values (@exchangeGuid, @eventName, @subscriberName, @creationDate, @active)",
            new {
                exchangeGuid = _exchangeGuid,
                eventName = _eventName,
                subscriberName = _subscriberName,
                creationDate = _creationDate,
                active = _active
            });
    }

    public ExchangeBuilder WithEventName(string value) 
    {
        _eventName = value;
        
        return this;
    }

    public ExchangeBuilder WithExchangeGuid(Guid value) 
    {
        _exchangeGuid = value;
        
        return this;
    }

    public ExchangeBuilder WithSubscriberName(string value) 
    {
        _subscriberName = value;
        
        return this;
    }
}

