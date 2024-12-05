using SOLTEC.Core.DataSources;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Extensions;
using SOLTEC.Core.Exceptions;
using SOLTEC.Core.MessageMQ.Interfaces;
using SOLTEC.Core.MessageMQ.Model;
using SOLTEC.MQApiBusiness.EventMessage.Models;

namespace SOLTEC.MQApiBusiness.Message.Infrastructure.Repositories;

public class MessagesRepositorySql : IMessageRepository 
{

    private SQLDataBase dataSource;

    public MessagesRepositorySql(string connectionConfig) 
    {
        dataSource = new SQLDataBase();
        dataSource.ConnectionConfig = connectionConfig;
    }

    public void Dispose() 
    {
        dataSource?.Dispose();
    }

    #region EventMessage

    public virtual async Task InsertEventMessageAsync(EventMessageModel eventModel) 
    {
        await dataSource.TransactionalQueryScalarAsync<int>(
              @"insert into MQ.EventMessage (EventMessageGuid, Publisher, EventName, Message, CreationDate, ProcessedDate) 
                    values (@eventMessageGuid, @publisher, @eventName, @message, @creationDate, @processedDate);
                select @@ROWCOUNT;", 
              new {
                  eventMessageGuid = eventModel.EventMessageGuid,
                  eventName = eventModel.EventName,
                  creationDate = eventModel.CreationDate,
                  message = eventModel.Message,
                  processedDate = eventModel.ProcessedDate,
                  publisher = eventModel.Publisher
              });
    }

    public virtual void InsertEventMessage(EventMessageModel eventModel) 
    {
        dataSource.TransactionalQueryScalar<int>(
              @"insert into MQ.EventMessage (EventMessageGuid, Publisher, EventName, Message, CreationDate, ProcessedDate) 
                    values (@eventMessageGuid, @publisher, @eventName, @message, @creationDate, @processedDate);
                select @@ROWCOUNT;",
              new {
                  eventMessageGuid = eventModel.EventMessageGuid,
                  eventName = eventModel.EventName,
                  creationDate = eventModel.CreationDate,
                  message = eventModel.Message,
                  processedDate = eventModel.ProcessedDate,
                  publisher = eventModel.Publisher
              });
    }

    public virtual async Task UpdateEventMessageAsync(EventMessageModel eventModel) 
    {
        await dataSource.TransactionalQueryScalarAsync<int>(
              @"update MQ.EventMessage set ProcessedDate = @processedDate
                    where EventMessageGuid = @eventMessageGuid;
                select @@ROWCOUNT;",
              new {
                  eventMessageGuid = eventModel.EventMessageGuid,
                  processedDate = eventModel.ProcessedDate
              });
    }

    public virtual void UpdateEventMessage(EventMessageModel eventModel) 
    {
        dataSource.TransactionalQueryScalar<int>(
              @"update MQ.EventMessage set ProcessedDate = @processedDate
                    where EventMessageGuid = @eventMessageGuid;
                select @@ROWCOUNT;",
              new {
                  eventMessageGuid = eventModel.EventMessageGuid,
                  processedDate = eventModel.ProcessedDate
              });
    }

    public virtual async Task<List<EventMessageModel>> GetUnprocessedEventMessagesAsync() 
    {
        var eventMessageModels = await dataSource.SelectAsync<dynamic>(
            @"select EventMessageGuid, EventName, Message, CreationDate, isNull(ProcessedDate,'') 'ProcessedDate'
                from MQ.EventMessage
                where isNull(processedDate,'') = ''
                order by id");
        return eventMessageModels == null ? new List<EventMessageModel>() : CreateEventMessageModels(eventMessageModels);
    }

    public virtual List<EventMessageModel> GetUnprocessedEventMessages() 
    {
        var eventMessageModels = dataSource.Select<dynamic>(
            @"select EventMessageGuid, EventName, Message, CreationDate, isNull(ProcessedDate,'') 'ProcessedDate'
                from MQ.EventMessage
                where isNull(processedDate,'') = ''
                order by id");
        return eventMessageModels == null ? new List<EventMessageModel>() : CreateEventMessageModels(eventMessageModels);
    }

    private List<EventMessageModel> CreateEventMessageModels(IList<dynamic> eventMessageModels) 
    {
        var result = new List<EventMessageModel>();
        foreach (var eventMessageModel in eventMessageModels) {
            result.Add(new EventMessageModel(eventMessageModel.EventMessageGuid, eventMessageModel.Publisher, eventMessageModel.EventName, eventMessageModel.CreationDate, eventMessageModel.Message, eventMessageModel.ProcessedDate));
        }
        return result;
    }

    public virtual async Task LinkEventAsync(LinkEventModel model) 
    {
        await dataSource.TransactionalQueryScalarAsync<int>(@"Insert into MQ.LinkedEvents (ParentId, EventId, Creation) values (@parentId, @eventId, @creation)",
            new {
                parentId = model.ParentId,
                eventId = model.EventId,
                creation = model.Creation
            }
        );
    }

    public virtual void LinkEvent(LinkEventModel model) 
    {
        dataSource.TransactionalQueryScalar<int>(@"Insert into MQ.LinkedEvents (ParentId, EventId, Creation) values (@parentId, @eventId, @creation)",
            new {
                parentId = model.ParentId,
                eventId = model.EventId,
                creation = model.Creation
            }
        );
    }

    #endregion

    #region Exchange

    public virtual async Task<List<string>> GetAssociatedActiveSubscribersToEventAsync(string eventName) 
    {
        var subscribers = await dataSource.SelectAsync<string>(@"                
            select SubscriberName
                from MQ.Exchange 
                where EventName = @eventName and Active = 1", new { eventName });
        return subscribers.ToList();
    }

    public virtual List<string> GetAssociatedActiveSubscribersToEvent(string eventName) 
    {
        var subscribers = dataSource.Select<string>(@"                
            select SubscriberName
                from MQ.Exchange 
                where EventName = @eventName and Active = 1", new { eventName });
        return subscribers.ToList();
    }

    #endregion

    #region Message

    public virtual async Task<List<MessageModel>> GetUnProcessedMessagesAsync(string subscriberName, string eventName) 
    {
        var messages = await dataSource.SelectAsync<MessageModel>(@"                
            select MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage
                from MQ.Message 
                where SubscriberName = @subscriberName and 
                    EventName = @eventName and
                    isnull(ProcessedDate,'') = '' 
                order by id",
            new {
                subscriberName,
                eventName
            });
        return messages.IsNullOrEmpty() ? new List<MessageModel>() : messages.ToList();
    }

    public virtual List<MessageModel> GetUnProcessedMessages(string subscriberName, string eventName) 
    {
        var messages = dataSource.Select<MessageModel>(@"                
            select MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage
                from MQ.Message 
                where SubscriberName = @subscriberName and 
                    EventName = @eventName and
                    isnull(ProcessedDate,'') = '' 
                order by id",
            new {
                subscriberName,
                eventName
            });
        return messages.IsNullOrEmpty() ? new List<MessageModel>() : messages.ToList();
    }

    public virtual async Task<MessageModel> GetMessageByAsync(Guid messageId) 
    {
        var messages = await dataSource.SelectAsync<MessageModel>(@"                
            select MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage
                from MQ.Message 
                where MessageGuid = @messageId",
            new {
                messageId
            });
        return messages.IsNullOrEmpty() ? throw new MessageMQException(EventMessageErrorEnum.MessageNotFound) : messages.ToList().First();
    }

    public virtual MessageModel GetMessageBy(Guid messageId) 
    {
        var messages = dataSource.Select<MessageModel>(@"                
            select MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage
                from MQ.Message 
                where MessageGuid = @messageId",
            new {
                messageId
            });
        return messages.IsNullOrEmpty() ? throw new MessageMQException(EventMessageErrorEnum.MessageNotFound) : messages.ToList().First();
    }

    public virtual async Task InsertMessageAsync(MessageModel messageModel) 
    {
        await dataSource.SelectScalarAsync<int>(@"                
            insert into MQ.Message (MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage)
                values(@messageGuid, @eventMessageGuid, @eventName, @subscriberName, @message, @creationDate, @processedDate, @statusCode, @errorMessage)
            select @@ROWCOUNT",
            new {
                messageGuid = messageModel.MessageGuid,
                eventMessageGuid = messageModel.EventMessageGuid,
                eventName = messageModel.EventName,
                subscriberName = messageModel.SubscriberName,
                message = messageModel.Message,
                creationDate = messageModel.CreationDate,
                processedDate = messageModel.ProcessedDate,
                statusCode = (int)messageModel.StatusCode,
                errorMessage = messageModel.ErrorMessage
            });
    }

    public virtual void InsertMessage(MessageModel messageModel) 
    {
        dataSource.SelectScalar<int>(@"                
            insert into MQ.Message (MessageGuid, EventMessageGuid, EventName, SubscriberName, Message, CreationDate, ProcessedDate, StatusCode, ErrorMessage)
                values(@messageGuid, @eventMessageGuid, @eventName, @subscriberName, @message, @creationDate, @processedDate, @statusCode, @errorMessage)
            select @@ROWCOUNT",
            new {
                messageGuid = messageModel.MessageGuid,
                eventMessageGuid = messageModel.EventMessageGuid,
                eventName = messageModel.EventName,
                subscriberName = messageModel.SubscriberName,
                message = messageModel.Message,
                creationDate = messageModel.CreationDate,
                processedDate = messageModel.ProcessedDate,
                statusCode = (int)messageModel.StatusCode,
                errorMessage = messageModel.ErrorMessage
            });
    }


    public virtual async Task UpdateMessageAsync(MessageModel messageModel) 
    {
        await dataSource.SelectScalarAsync<int>(@"                
            update MQ.Message set 
                    ProcessedDate = @processedDate,
                    StatusCode = @statusCode,
                    ErrorMessage = @errorMessage
                where MessageGuid = @messageGuid
            select @@ROWCOUNT",
            new {
                messageGuid = messageModel.MessageGuid,
                processedDate = messageModel.ProcessedDate,
                statusCode = messageModel.StatusCode,
                errorMessage = messageModel.ErrorMessage
            });
    }

    public virtual void UpdateMessage(MessageModel messageModel) 
    {
        dataSource.SelectScalar<int>(@"                
            update MQ.Message set 
                    ProcessedDate = @processedDate,
                    StatusCode = @statusCode,
                    ErrorMessage = @errorMessage
                where MessageGuid = @messageGuid
            select @@ROWCOUNT",
            new {
                messageGuid = messageModel.MessageGuid,
                processedDate = messageModel.ProcessedDate,
                statusCode = messageModel.StatusCode,
                errorMessage = messageModel.ErrorMessage
            });
    }
    #endregion
}
