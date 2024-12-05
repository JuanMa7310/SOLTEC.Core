using SOLTEC.Core.Extensions;
using SOLTEC.Core.MessageMQ.Commands;
using SOLTEC.Core.MessageMQ.Model;
using SOLTEC.Core.MessageMQ.Responses;
using SOLTEC.MQApiBusiness.EventMessage.Models;
using System.Net;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using SOLTEC.Core.MessageMQ.Interfaces;

namespace SOLTEC.Core.MessageMQ;

public class Message : IMessage 
{
    private readonly IMessageRepository _repository;

    public Message(IMessageRepository repository) 
    {
        _repository = repository;
    }

    public virtual async Task<Guid> PushAsync(MessageCommand messageCommand) 
    {
        var message = new EventMessageModel(messageCommand.Publisher, messageCommand.EventName, messageCommand.Message);
        await _repository.InsertEventMessageAsync(message);
        return message.EventMessageGuid;
    }

    public virtual Guid Push(MessageCommand messageCommand) 
    {
        var message = new EventMessageModel(messageCommand.Publisher, messageCommand.EventName, messageCommand.Message);
        _repository.InsertEventMessage(message);
        return message.EventMessageGuid;
    }

    public virtual async Task LinkEventAsync(LinkedEventCommand command) 
    {
        var model = new LinkEventModel(command.ParentId, command.EventId);
        await _repository.LinkEventAsync(model);
    }

    public virtual void LinkEvent(LinkedEventCommand command) 
    {
        var model = new LinkEventModel(command.ParentId, command.EventId);
        _repository.LinkEvent(model);
    }

    public virtual async Task<List<MessageResponse>> PopAsync(string subscriber, string eventName) {
        var modelMessages = await _repository.GetUnProcessedMessagesAsync(subscriber, eventName);
        if (modelMessages.IsNullOrEmpty()) 
            return new List<MessageResponse>();
        return await ProcessMessageResponseAsync(modelMessages);
    }

    public virtual List<MessageResponse> Pop(string subscriber, string eventName) 
    {
        var modelMessages = _repository.GetUnProcessedMessages(subscriber, eventName);
        if (modelMessages.IsNullOrEmpty()) 
            return new List<MessageResponse>();
        return ProcessMessageResponse(modelMessages);
    }

    public virtual async Task ExchangeProcessAsync() 
    {
        var eventMessageModels = await _repository.GetUnprocessedEventMessagesAsync();
        if (eventMessageModels.IsNullOrEmpty()) 
            return;
        await TryProcessAllEventMessages(eventMessageModels);
    }

    public virtual async Task ChangeStatusProcessingAsync(Guid messageId, HttpStatusCode statusCode, 
        string errorMessage) 
    {
        var messageModel = await _repository.GetMessageByAsync(messageId);
        messageModel.UpdateStatusCode(statusCode, errorMessage);
        await _repository.UpdateMessageAsync(messageModel);
    }

    public virtual void ChangeStatusProcessing(Guid messageId, HttpStatusCode statusCode, string errorMessage) 
    {
        var messageModel = _repository.GetMessageBy(messageId);
        messageModel.UpdateStatusCode(statusCode, errorMessage);
        _repository.UpdateMessage(messageModel);
    }

    public virtual void ExchangeProcess() 
    {
        var eventMessageModels = _repository.GetUnprocessedEventMessages();
        ValidateEventMessageModelResponse(eventMessageModels);
        var eventMessageModel = eventMessageModels.First();
        TryInsertForEachSubscriptor(eventMessageModel);
        eventMessageModel.UpdateToProcessed();
        _repository.UpdateEventMessage(eventMessageModel);
    }

    public virtual void Dispose() 
    {
        _repository.Dispose();
    }

    private static void ValidateEventMessageModelResponse(List<EventMessageModel> eventMessageModels) {
        if (eventMessageModels.IsNullOrEmpty()) 
            throw new MessageMQException(EventMessageErrorEnum.NotEventMessage, HttpStatusCode.NoContent);
    }

    private async Task TryInsertForEachSubscriptorAsync(EventMessageModel eventMessageModel) 
    {
        var subscripters = await _repository.GetAssociatedActiveSubscribersToEventAsync(eventMessageModel.EventName);
        subscripters.ForEach(async void (s) => {
            var messageModel = new MessageModel(eventMessageModel.EventMessageGuid, eventMessageModel.EventName, s, eventMessageModel.Message);
            await _repository.InsertMessageAsync(messageModel);
        });
    }

    private void TryInsertForEachSubscriptor(EventMessageModel eventMessageModel) 
    {
        var subscripters = _repository.GetAssociatedActiveSubscribersToEvent(eventMessageModel.EventName);
        subscripters.ForEach(s => {
            var messageModel = new MessageModel(eventMessageModel.EventMessageGuid, eventMessageModel.EventName, s, eventMessageModel.Message);
            _repository.InsertMessage(messageModel);
        });
    }

    private async Task<List<MessageResponse>> ProcessMessageResponseAsync(List<MessageModel> modelMessages) 
    {
        var result = new List<MessageResponse>();
        foreach (var messageModel in modelMessages) 
        {
            messageModel.UpdateProcessed();
            await _repository.UpdateMessageAsync(messageModel);
            result.Add(CreateMessageResponse(messageModel));
        }
        return result;
    }

    private List<MessageResponse> ProcessMessageResponse(List<MessageModel> modelMessages) 
    {
        var result = new List<MessageResponse>();
        foreach (var messageModel in modelMessages) 
        {
            messageModel.UpdateProcessed();
            _repository.UpdateMessage(messageModel);
            result.Add(CreateMessageResponse(messageModel));
        }
        return result;
    }

    private MessageResponse CreateMessageResponse(MessageModel modelMessage) 
    {
        return new MessageResponse { MessageId = modelMessage.MessageGuid, Message = modelMessage.Message, EventMessageGuid = modelMessage.EventMessageGuid };
    }

    private async Task TryProcessAllEventMessages(List<EventMessageModel> eventMessageModels) 
    {
        foreach (var eventMessag in eventMessageModels) 
        {
            await TryInsertForEachSubscriptorAsync(eventMessag);
            eventMessag.UpdateToProcessed();
            await _repository.UpdateEventMessageAsync(eventMessag);
        }
    }
}

