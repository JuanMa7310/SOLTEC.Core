using SOLTEC.Core.DataSources.Interfaces;

namespace SOLTEC.Core.Tests.MessageMQ.Builders;

public class EventMessageBuilder 
{
    private readonly IDataSource _dataSource;
    private Guid _eventMessageGuid = DefaultValues.EventMessageGuid;
    private string _publisher = DefaultValues.Publisher;
    private string _eventName = DefaultValues.EventName;
    private string _message = DefaultValues.Message;
    private string _creationDate = DefaultValues.CreationWithTime;
    private string _processedDate = string.Empty;
    
    public EventMessageBuilder(IDataSource dataSource) 
    {
        _dataSource = dataSource;
    }

    public void Build() 
    {
        _dataSource.Execute(@"
            insert into MQ.EventMessage (EventMessageGuid,
                                          Publisher,
                                          EventName,
                                          Message,
                                          CreationDate,
                                          ProcessedDate)
                values (@eventMessageGuid,
                        @publisher,
                        @eventName,                            
                        @message,                            
                        @creationDate,
                        @processedDate)",
            new {
                eventMessageGuid = _eventMessageGuid,
                publisher = _publisher,
                eventName = _eventName,
                message = _message,
                creationDate = _creationDate,
                processedDate = _processedDate
            });
    }

    public EventMessageBuilder WithEventMessageGuid(Guid value) 
    {
        _eventMessageGuid = value;
        return this;
    }

    public EventMessageBuilder WithEventName(string value) 
    {
        _eventName = value;
        return this;
    }

    public EventMessageBuilder WithMessage(string value) 
    {
        _message = value;
        return this;
    }

    public EventMessageBuilder WithCreationDate(string value) 
    {
        _creationDate = value;
        return this;
    }
}
