using SOLTEC.Core.MessageMQ;
using SOLTEC.Core.MessageMQ.Commands;
using SOLTEC.Core.Exceptions;
using SOLTEC.Core.MessageMQ.Model;
using SOLTEC.Core.MessageMQ.Responses;
using SOLTEC.Core.Tests.Factory;
using SOLTEC.MQApiBusiness.EventMessage.Models;
using SOLTEC.MQApiBusiness.Message.Infrastructure.Repositories;
using FluentAssertions;
using NSubstitute;
using System.Net;
using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Tests.MessageMQ.Actions;

public class MessageShould 
{
    private MessagesRepositorySql _messagesRepositorySql;
    private Message _message;

    [SetUp]
    public void SetUp() 
    {
        _messagesRepositorySql = SubstituteFactory.MessagesRepositorySql();
        _message = new Message(_messagesRepositorySql);
    }

    [Test]
    public async Task push_message_for_a_publisher_and_an_event_name() 
    {
        var messageCommand = CreateMessageCommand();

        await _message.PushAsync(messageCommand);

        await ValidatePushedMessage(messageCommand);
    }

    [Test]
    public async Task link_events_async() 
    {
        var command = CreateLinkedEventCommand();

        await _message.LinkEventAsync(command);

        await _messagesRepositorySql.Received(1).LinkEventAsync(VaidateLinkEventModel(command));
    }

    [Test]
    public void link_events() 
    {
        var command = CreateLinkedEventCommand();

        _message.LinkEvent(command);

        _messagesRepositorySql.Received(1).LinkEvent(VaidateLinkEventModel(command));
    }

    [Test]
    public async Task pop_message_for_a_subscriber_and_an_eventName_async() 
    {
        var messageModel = CreateMessageModel();

        var messageResponse = await _message.PopAsync(DefaultValues.SubscriberName, DefaultValues.EventName);

        await ValidatePopAsyncMessageResponse(messageModel, messageResponse);
    }

    [Test]
    public void pop_message_for_a_subscriber_and_an_eventName() 
    {
        var messageModel = CreateMessageModel();

        var messageResponse = _message.Pop(DefaultValues.SubscriberName, DefaultValues.EventName);
        
        ValidatePopMessageResponse(messageModel, messageResponse);
    }

    [Test]
    public async Task return_empty_message_response_list_when_not_there_is_any_message_for_a_subscriberAsync() 
    {
        _messagesRepositorySql.GetUnProcessedMessagesAsync(DefaultValues.SubscriberName, DefaultValues.EventName)
            .Returns(new List<MessageModel>());

        var messageResponse = await _message.PopAsync(DefaultValues.SubscriberName, 
            DefaultValues.EventName);

        messageResponse.Should().BeEmpty();
    }

    [Test]
    public void return_empty_message_response_list_when_not_there_is_any_message_for_a_subscriber() 
    {
        _messagesRepositorySql.GetUnProcessedMessages(DefaultValues.SubscriberName, DefaultValues.EventName)
            .Returns(new List<MessageModel>());

        var messageResponse = _message.Pop(DefaultValues.SubscriberName, DefaultValues.EventName);

        messageResponse.Should().BeEmpty();
    }

    [Test]
    public async Task process_event_menssages_unprocessed_and_create_message_by_sunscriptor_async() 
    {
        var eventMessageModel = CreateEventMessageModel();
        _messagesRepositorySql.GetUnprocessedEventMessagesAsync()
            .Returns(new List<EventMessageModel>() { eventMessageModel });
        _messagesRepositorySql.GetAssociatedActiveSubscribersToEventAsync(DefaultValues.EventName)
            .Returns(new List<string> { DefaultValues.SubscriberName });

        await _message.ExchangeProcessAsync();

        await _messagesRepositorySql.Received(1).GetUnprocessedEventMessagesAsync();
        await _messagesRepositorySql.Received(1).GetAssociatedActiveSubscribersToEventAsync(DefaultValues.EventName);
        await _messagesRepositorySql.Received(1).InsertMessageAsync(ValidateInsertedMessage(eventMessageModel));
        await _messagesRepositorySql.Received(1).UpdateEventMessageAsync(ValidateUpdatedEventMessage(eventMessageModel));
    }
    
    [Test]
    public void process_event_menssages_unprocessed_and_create_message_by_sunscriptor() 
    {
        var eventMessageModel = CreateEventMessageModel();
        _messagesRepositorySql.GetUnprocessedEventMessages()
            .Returns(new List<EventMessageModel>() { eventMessageModel });
        _messagesRepositorySql.GetAssociatedActiveSubscribersToEvent(DefaultValues.EventName)
            .Returns(new List<string> { DefaultValues.SubscriberName });

        _message.ExchangeProcess();

        _messagesRepositorySql.Received(1).GetUnprocessedEventMessages();
        _messagesRepositorySql.Received(1).GetAssociatedActiveSubscribersToEvent(DefaultValues.EventName);
        _messagesRepositorySql.Received(1).InsertMessage(ValidateInsertedMessage(eventMessageModel));
        _messagesRepositorySql.Received(1).UpdateEventMessage(ValidateUpdatedEventMessage(eventMessageModel));
    }

    [Test]
    public async Task change_status_and_status_message_for_a_processed_message_async() 
    {
        var messageModel = CreateMessageModel();
        _messagesRepositorySql.GetMessageByAsync(messageModel.MessageGuid).Returns(messageModel);

        await _message.ChangeStatusProcessingAsync(messageModel.MessageGuid, DefaultValues.HttpStatusCode,
            DefaultValues.ErrorMessage);

        await _messagesRepositorySql.Received(1)
            .UpdateMessageAsync(Arg.Is<MessageModel>( x => 
                x.MessageGuid.Equals(messageModel.MessageGuid) && 
                x.StatusCode.Equals(messageModel.StatusCode) && x.ErrorMessage.Equals(messageModel.ErrorMessage)));
    }

    [Test]
    public void change_status_and_status_message_for_a_processed_message() 
    {
        var messageModel = CreateMessageModel();
        _messagesRepositorySql.GetMessageBy(messageModel.MessageGuid).Returns(messageModel);

        _message.ChangeStatusProcessing(messageModel.MessageGuid, DefaultValues.HttpStatusCode, 
            DefaultValues.ErrorMessage);

        _messagesRepositorySql.Received(1)
            .UpdateMessage( Arg.Is<MessageModel>( x => x.MessageGuid.Equals(messageModel.MessageGuid) 
            && x.StatusCode.Equals(messageModel.StatusCode) && x.ErrorMessage.Equals(messageModel.ErrorMessage)));
    }

    [Ignore("It is not a mistake not to have a event message.")]
    [Test]
    public async Task throw_exception_with_NotEventMessage_reason_when_not_there_is_any_event_message_to_process_async() 
    {
        _messagesRepositorySql.GetUnprocessedEventMessages()
            .Returns(new List<EventMessageModel>());

        var result = async () => await _message.ExchangeProcessAsync();

        result.Should().ThrowAsync<MessageMQException>().Result.And.Reason.Should()
            .Be(EventMessageErrorEnum.NotEventMessage.ToString());
        result.Should().ThrowAsync<MessageMQException>().Result.And.HttpStatusCode.Should()
            .Be(HttpStatusCode.NoContent);
    }

    [Test]
    public void throw_exception_with_NotEventMessage_reason_when_not_there_is_any_event_message_to_process() 
    {
        _messagesRepositorySql.GetUnprocessedEventMessages().Returns(new List<EventMessageModel>());

        var result = () => _message.ExchangeProcess();

        result.Should().Throw<MessageMQException>().And.Reason.Should()
            .Be(EventMessageErrorEnum.NotEventMessage.ToString());
        result.Should().Throw<MessageMQException>().And.HttpStatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private void ValidatePopMessageResponse(MessageModel messageModel, List<MessageResponse> messageResponse) 
    {
        messageResponse.Count.Should().Be(1);
        messageResponse.First().Message.Should().Be(messageModel.Message);
        messageResponse.First().MessageId.Should().Be(messageModel.MessageGuid);
        messageResponse.First().EventMessageGuid.Should().Be(messageModel.EventMessageGuid);
        _messagesRepositorySql.Received(1).GetUnProcessedMessages(DefaultValues.SubscriberName, DefaultValues.EventName);
        _messagesRepositorySql.Received(1).UpdateMessage(ValidateUpdatedMessageModel());
    }

    private async Task ValidatePopAsyncMessageResponse(MessageModel messageModel, List<MessageResponse> messageResponse) 
    {
        messageResponse.Count.Should().Be(1);
        messageResponse.First().Message.Should().Be(messageModel.Message);
        messageResponse.First().MessageId.Should().Be(messageModel.MessageGuid);
        messageResponse.First().EventMessageGuid.Should().Be(messageModel.EventMessageGuid);
        await _messagesRepositorySql.Received(1).GetUnProcessedMessagesAsync(DefaultValues.SubscriberName, 
            DefaultValues.EventName);
        await _messagesRepositorySql.Received(1).UpdateMessageAsync(ValidateUpdatedMessageModel());
    }

    private EventMessageModel CreateEventMessageModel() 
    {
        return new EventMessageModel(DefaultValues.EventMessageGuid, DefaultValues.Publisher, DefaultValues.EventName, DefaultValues.CreationDate, DefaultValues.Message, string.Empty);
    }

    private static EventMessageModel ValidateUpdatedEventMessage(EventMessageModel eventMessageModel) {
        return Arg.Is<EventMessageModel>(e => e.EventMessageGuid.Equals(eventMessageModel.EventMessageGuid) &&
                                              e.Publisher.Equals(eventMessageModel.Publisher) &&
                                              e.EventName.Equals(eventMessageModel.EventName) &&
                                              e.CreationDate.Equals(eventMessageModel.CreationDate) &&
                                              !string.IsNullOrWhiteSpace(e.ProcessedDate));
    }

    private MessageModel ValidateInsertedMessage(EventMessageModel eventMessageModel) 
    {
        return Arg.Is<MessageModel>(x => x.EventMessageGuid.Equals(eventMessageModel.EventMessageGuid) &&
                                         x.EventName.Equals(eventMessageModel.EventName) &&
                                         x.SubscriberName.Equals(DefaultValues.SubscriberName) &&
                                         x.Message.Equals(eventMessageModel.Message) &&
                                         !string.IsNullOrWhiteSpace(x.CreationDate) &&
                                         x.ProcessedDate.Equals(string.Empty));
    }

    private MessageModel ValidateUpdatedMessageModel() 
    {
        return Arg.Is<MessageModel>(x => x.MessageGuid.Equals(DefaultValues.MessageGuid) &&
                                         x.EventMessageGuid.Equals(DefaultValues.EventMessageGuid) &&
                                         x.EventName.Equals(DefaultValues.EventName) &&
                                         x.SubscriberName.Equals(DefaultValues.SubscriberName) &&
                                         x.Message.Equals(DefaultValues.Message) &&
                                         x.CreationDate.Equals(DefaultValues.CreationDate) &&
                                         !string.IsNullOrWhiteSpace(x.ProcessedDate));
    }

    private static LinkEventModel VaidateLinkEventModel(LinkedEventCommand command) 
    {
        return Arg.Is<LinkEventModel>(x => x.ParentId.Equals(command.ParentId) &&
                                           x.EventId.Equals(command.EventId));
    }

    private MessageCommand CreateMessageCommand() 
    {
        return new MessageCommand {
            Publisher = DefaultValues.Publisher,
            EventName = DefaultValues.EventName,
            Message = DefaultValues.Message
        };
    }

    private async Task ValidatePushedMessage(MessageCommand messageCommand) 
    {
        await _messagesRepositorySql.Received(1)
            .InsertEventMessageAsync(Arg.Is<EventMessageModel>(x => x.Publisher.Equals(messageCommand.Publisher) && 
                                                                    x.EventName.Equals(messageCommand.EventName) && 
                                                                    x.Message.Equals(messageCommand.Message) && 
                                                                    x.ProcessedDate.Equals(string.Empty)));
    }

    private MessageModel CreateMessageModel() 
    {
        var messageModel = new MessageModel(DefaultValues.MessageGuid, DefaultValues.EventMessageGuid, 
            DefaultValues.EventName, DefaultValues.SubscriberName, DefaultValues.Message, DefaultValues.CreationDate, 
            string.Empty, (int)DefaultValues.HttpStatusCode, DefaultValues.ErrorMessage);
        _messagesRepositorySql.GetUnProcessedMessages(DefaultValues.SubscriberName, DefaultValues.EventName)
            .Returns(new List<MessageModel>() { messageModel });
        _messagesRepositorySql.GetUnProcessedMessagesAsync(DefaultValues.SubscriberName, DefaultValues.EventName)
            .Returns(new List<MessageModel>() { messageModel });
        return messageModel;
    }

    private LinkedEventCommand CreateLinkedEventCommand() 
    {
        return new LinkedEventCommand 
        {
            ParentId = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
        };
    }
}
