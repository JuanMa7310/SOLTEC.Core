using System.Net;
using SOLTEC.Core.DataSources.Interfaces;

namespace SOLTEC.Core.Tests.MessageMQ.Builders;

public class MessageBuilder 
{
    private readonly IDataSource _dataSource;
    private Guid _messageGuid = DefaultValues.MessageGuid;
    private Guid _eventMessageGuid = DefaultValues.EventMessageGuid;
    private string _eventName = DefaultValues.EventName;
    private string _subscriberName = DefaultValues.SubscriberName;
    private string _message = DefaultValues.Message;
    private string _creationDate = DefaultValues.CreationWithTime;
    private string _processedDate = string.Empty;
    private HttpStatusCode _statusCode = DefaultValues.HttpStatusCode;
    private string _errorMessage = string.Empty;

    public MessageBuilder(IDataSource dataSource) 
    {
        _dataSource = dataSource;
    }

    public void Build() 
    {
        _dataSource.Execute(@"
            insert into MQ.Message (
                    MessageGuid,                        
                    EventMessageGuid, 
                    EventName,
                    SubscriberName,
                    Message,
                    CreationDate,
                    ProcessedDate,
                    StatusCode, 
                    ErrorMessage) 
                values (
                    @messageGuid,
                    @eventMessageGuid, 
                    @eventName,
                    @subscriberName,
                    @message,
                    @creationDate, 
                    @processedDate,
                    @statusCode,
                    @errorMessage)",
            new {
                messageGuid = _messageGuid,
                eventMessageGuid = _eventMessageGuid,
                eventName = _eventName,
                subscriberName = _subscriberName,
                message = _message,
                creationDate = _creationDate,
                processedDate = _processedDate,
                statusCode = _statusCode,
                errorMessage = _errorMessage
            });
    }

    public MessageBuilder WithMessageGuid(Guid value) 
    {
        _messageGuid = value;
        
        return this;
    }

    public MessageBuilder WithEventMessageGuid(Guid value) 
    {
        _eventMessageGuid = value;
        
        return this;
    }

    public MessageBuilder WithEventName(string value) 
    {
        _eventName = value;
        
        return this;
    }

    public MessageBuilder WithSubscriberName(string value) 
    {
        _subscriberName = value;
        
        return this;
    }

    public MessageBuilder WithMessage(string value) 
    {
        _message = value;
        
        return this;
    }

    public MessageBuilder WithCreationDate(string value) 
    {
        _creationDate = value;
        
        return this;
    }

    public MessageBuilder WithProcessedDate(string value) 
    {
        _processedDate = value;
        
        return this;
    }
}
