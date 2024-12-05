using SOLTEC.Core.MessageMQ.Model;
using SOLTEC.MQApiBusiness.EventMessage.Models;

namespace SOLTEC.Core.MessageMQ.Interfaces;

public interface IMessageRepository 
{
    Task InsertEventMessageAsync(EventMessageModel eventModel);
    void InsertEventMessage(EventMessageModel eventModel);
    Task UpdateEventMessageAsync(EventMessageModel eventModel);        
    void UpdateEventMessage(EventMessageModel eventModel);        
    Task<List<EventMessageModel>> GetUnprocessedEventMessagesAsync();
    List<EventMessageModel> GetUnprocessedEventMessages();
    Task<List<string>> GetAssociatedActiveSubscribersToEventAsync(string eventName);
    List<string> GetAssociatedActiveSubscribersToEvent(string eventName);
    Task<List<MessageModel>> GetUnProcessedMessagesAsync(string subscriberName, string eventName);
    List<MessageModel> GetUnProcessedMessages(string subscriberName, string eventName);
    Task InsertMessageAsync(MessageModel messageModel);
    void InsertMessage(MessageModel messageModel);
    Task UpdateMessageAsync(MessageModel messageModel);
    void UpdateMessage(MessageModel messageModel);
    void Dispose();
    Task<MessageModel> GetMessageByAsync(Guid messageId);
    MessageModel GetMessageBy(Guid messageId);
    Task LinkEventAsync(LinkEventModel linkEventModel);
    void LinkEvent(LinkEventModel linkEventModel);
}
