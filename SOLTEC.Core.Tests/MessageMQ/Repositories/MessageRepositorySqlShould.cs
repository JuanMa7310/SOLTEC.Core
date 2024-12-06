using System.Net;
using FluentAssertions;
using MoreLinq;
using SOLTEC.Core.DataSources;
using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;
using SOLTEC.Core.MessageMQ.Model;
using SOLTEC.Core.Tests.MessageMQ.Builders;
using SOLTEC.MQApiBusiness.EventMessage.Models;
using SOLTEC.MQApiBusiness.Message.Infrastructure.Repositories;

namespace SOLTEC.Core.Tests.MessageMQ.Repositories;

public class MessageRepositorySqlShould
{
    private MessagesRepositorySql _messagesRepositorySql;
    private MessageBuilder _messageBuilder;
    private ExchangeBuilder _exchangeBuilder;
    private EventMessageBuilder _eventMessageBuilder;
    private EventMessageModel _eventModel;
    private string _connectionString;
    private SQLDataBase _sQlDataBase;

    public MessageRepositorySqlShould()
    {
        _connectionString =
            "Data Source=localhost;Initial Catalog=SOLTECMQ;" +
            "User ID=testUser;Password=1234_asdf;MultipleActiveResultSets = True;";
    }

    [SetUp]
    public void SetUpTest() 
    {
        _sQlDataBase = new SQLDataBase();
        _sQlDataBase.ConnectionConfig = _connectionString;
        _messageBuilder = new MessageBuilder(_sQlDataBase);
        _exchangeBuilder = new ExchangeBuilder(_sQlDataBase);
        _eventMessageBuilder = new EventMessageBuilder(_sQlDataBase);
        _messagesRepositorySql = new MessagesRepositorySql(_connectionString);
    }

    [TearDown]
    public void TearDown() 
    {
        _sQlDataBase.TransactionalQuery<dynamic>(@"delete from MQ.Message");
        _sQlDataBase.TransactionalQuery<dynamic>(@"delete from MQ.Exchange");
        _sQlDataBase.TransactionalQuery<dynamic>(@"delete from MQ.EventMessage");
        _sQlDataBase.TransactionalQuery<dynamic>(@"delete from MQ.Event");
        _sQlDataBase.TransactionalQuery<dynamic>(@"delete from MQ.Exchange");
        _sQlDataBase.Dispose();
    }

    [Test]
    public async Task get_message_by_id_async() 
    {
        var expectedMessage = CreateAMessageModel();
        GivenAMessage(expectedMessage.MessageGuid, expectedMessage.SubscriberName, expectedMessage.EventName, 
            string.Empty);
        var messageModel = await _messagesRepositorySql.GetMessageByAsync(expectedMessage.MessageGuid);
        ValidateMessage(expectedMessage, messageModel);
    }

    [Test]
    public async Task throw_exception_when_not_exist_a_message_for_a_messageId_async() 
    {
        var result = async () => await _messagesRepositorySql.GetMessageByAsync(DefaultValues.MessageGuid);
        result.Should().ThrowAsync<MessageMQException>().Result.And.Reason.Should()
            .Be(EventMessageErrorEnum.MessageNotFound.ToString());
    }

    [Test]
    public void get_message_by_id() 
    {
        var expectedMessage = CreateAMessageModel();
        GivenAMessage(expectedMessage.MessageGuid, expectedMessage.SubscriberName, expectedMessage.EventName, 
            string.Empty);
        var messageModel = _messagesRepositorySql.GetMessageBy(expectedMessage.MessageGuid);
        ValidateMessage(expectedMessage, messageModel);
    }

    [Test]
    public async Task throw_exception_when_not_exist_a_message_for_a_messageId() 
    {
        Action result = () => _messagesRepositorySql.GetMessageBy(DefaultValues.MessageGuid);
        result.Should().Throw<MessageMQException>().And.Reason.Should()
            .Be(EventMessageErrorEnum.MessageNotFound.ToString());
    }

    [Test]
    public async Task get_first_not_processed_message_for_a_subscriber_and_an_eventName_async() {
        var expectedMessage = CreateAMessageModel();
        GivenAMessage(Guid.NewGuid(), expectedMessage.SubscriberName, "OtherEventName", 
            string.Empty);
        GivenAMessage(Guid.NewGuid(), expectedMessage.SubscriberName, expectedMessage.EventName, 
            DefaultValues.ProcessedDate);
        GivenAMessage(expectedMessage.MessageGuid, expectedMessage.SubscriberName, expectedMessage.EventName, 
            string.Empty);
        var messageModel = await _messagesRepositorySql
            .GetUnProcessedMessagesAsync(expectedMessage.SubscriberName, expectedMessage.EventName);
        ValidateMessage(expectedMessage, messageModel.First());
    }

    [Test]
    public void get_first_not_processed_message_for_a_subscriber_and_an_eventName() 
    {
        var expectedMessage = CreateAMessageModel();
        GivenAMessage(Guid.NewGuid(), expectedMessage.SubscriberName, "OtherEventName", 
            string.Empty);
        GivenAMessage(Guid.NewGuid(), expectedMessage.SubscriberName, expectedMessage.EventName,
            DefaultValues.ProcessedDate);
        GivenAMessage(expectedMessage.MessageGuid, expectedMessage.SubscriberName, expectedMessage.EventName, 
            string.Empty);
        var messageModel = _messagesRepositorySql
            .GetUnProcessedMessages(expectedMessage.SubscriberName, expectedMessage.EventName);
        ValidateMessage(expectedMessage, messageModel.First());
    }

    [Test]
    public async Task update_message_async() 
    {
        var expectedUpdatedMessage = CreateAMessageModel();
        GivenAMessage(expectedUpdatedMessage.MessageGuid, expectedUpdatedMessage.SubscriberName, 
            expectedUpdatedMessage.EventName, string.Empty);
        expectedUpdatedMessage.UpdateStatusCode(HttpStatusCode.Forbidden, DefaultValues.ErrorMessage);
        await _messagesRepositorySql.UpdateMessageAsync(expectedUpdatedMessage);
        await ValidateUpdateAsync(expectedUpdatedMessage);
    }

    [Test]
    public void update_message() 
    {
        var expectedUpdatedMessage = CreateAMessageModel();
        GivenAMessage(expectedUpdatedMessage.MessageGuid, expectedUpdatedMessage.SubscriberName, 
            expectedUpdatedMessage.EventName, string.Empty);
        expectedUpdatedMessage.UpdateStatusCode(HttpStatusCode.Forbidden, DefaultValues.ErrorMessage);
        _messagesRepositorySql.UpdateMessage(expectedUpdatedMessage);
        ValidateUpdate(expectedUpdatedMessage);
    }

    [Test]
    public async Task insert_message_for_a_subscriptor_async() 
    {
        var messageModel = CreateAMessageModel();
        await _messagesRepositorySql.InsertMessageAsync(messageModel);
        var messagesCount = await _sQlDataBase
            .SelectScalarAsync<int>(
                @" Select count(0) from AMKMQ.MQ.Message where MessageGuid = @messageGuid", 
                new { 
                    messageGuid = messageModel.MessageGuid 
                });
        messagesCount.Should().Be(1);
    }

    [Test]
    public void insert_message_for_a_subscriptor() 
    {
        var messageModel = CreateAMessageModel();
        _messagesRepositorySql.InsertMessage(messageModel);
        var messagesCount = _sQlDataBase
            .SelectScalar<int>(
                @" Select count(0) from AMKMQ.MQ.Message where MessageGuid = @messageGuid", 
                new { 
                    messageGuid = messageModel.MessageGuid 
                });
        messagesCount.Should().Be(1);
    }

    [Test]
    public async Task get_associated_sunscriptors_to_event_async() {
        GivenAnEventMessage();
        GivenAExchange(DefaultValues.SubscriberName);
        GivenAExchange("OtherSubscriber");
        var subscriptors = await _messagesRepositorySql
            .GetAssociatedActiveSubscribersToEventAsync(DefaultValues.EventName);
        subscriptors.Count.Should().Be(2);
        subscriptors.Any(s => s.Equals(DefaultValues.SubscriberName)).Should().BeTrue();
        subscriptors.Any(s => s.Equals("OtherSubscriber")).Should().BeTrue();
    }

    [Test]
    public void get_associated_sunscriptors_to_event() 
    {
        GivenAnEventMessage();
        GivenAExchange(DefaultValues.SubscriberName);
        GivenAExchange("OtherSubscriber");
        var subscriptors = _messagesRepositorySql.GetAssociatedActiveSubscribersToEvent(DefaultValues.EventName);
        subscriptors.Count.Should().Be(2);
        subscriptors.Any(s => s.Equals(DefaultValues.SubscriberName)).Should().BeTrue();
        subscriptors.Any(s => s.Equals("OtherSubscriber")).Should().BeTrue();
    }

    [Test]
    public async Task get_first_unprocessed_message_event_async() 
    {
        var firstEventMessageGuid = Guid.NewGuid();
        var secondEventMessageGuid = Guid.NewGuid();
        var expectedEventMessageModel = new EventMessageModel(firstEventMessageGuid, DefaultValues.Publisher, 
            DefaultValues.EventName, DefaultValues.CreationWithTime, DefaultValues.Message, string.Empty);
        GivenAnEventMessage(expectedEventMessageModel.EventMessageGuid, expectedEventMessageModel.EventName, 
            expectedEventMessageModel.Message, DefaultValues.CreationWithTime);
        GivenAnEventMessage(secondEventMessageGuid, DefaultValues.OtherEventName, DefaultValues.Message, 
            DefaultValues.CreationWithTime);
        var eventMessageDto = await _messagesRepositorySql.GetUnprocessedEventMessagesAsync();
        ValidateEventMessage(eventMessageDto.First(), expectedEventMessageModel);
    }

    [Test]
    public void get_first_unprocessed_message_event() 
    {
        var firstEventMessageGuid = Guid.NewGuid();
        var secondEventMessageGuid = Guid.NewGuid();
        var expectedEventMessageModel = new EventMessageModel(firstEventMessageGuid, DefaultValues.Publisher, 
            DefaultValues.EventName, DefaultValues.CreationWithTime, DefaultValues.Message, string.Empty);
        GivenAnEventMessage(expectedEventMessageModel.EventMessageGuid, expectedEventMessageModel.EventName, 
            expectedEventMessageModel.Message, DefaultValues.CreationWithTime);
        GivenAnEventMessage(secondEventMessageGuid, DefaultValues.OtherEventName, DefaultValues.Message, 
            DefaultValues.CreationWithTime);
        var eventMessageDto = _messagesRepositorySql.GetUnprocessedEventMessages();
        ValidateEventMessage(eventMessageDto.First(), expectedEventMessageModel);
    }

    [Test]
    public async Task insert_an_event_message_async() 
    {
        _eventModel = new EventMessageModel(DefaultValues.Publisher, DefaultValues.EventName, DefaultValues.Message);
        await _messagesRepositorySql.InsertEventMessageAsync(_eventModel);
        var @event = await _sQlDataBase.SelectAsync<dynamic>(@"Select top 1 * from AMKMQ.MQ.EventMessage");
        ValidateEvent(_eventModel, @event.First());
    }

    [Test]
    public void insert_an_event_message() 
    {
        _eventModel = new EventMessageModel(DefaultValues.Publisher, DefaultValues.EventName, DefaultValues.Message);
        _messagesRepositorySql.InsertEventMessage(_eventModel);
        var @event = _sQlDataBase.Select<dynamic>(@"Select top 1 * from AMKMQ.MQ.EventMessage");
        ValidateEvent(_eventModel, @event.First());
    }

    [Test]
    public async Task update_an_event_message_async() {
        GivenAnEventMessage(DefaultValues.EventMessageGuid, DefaultValues.EventName, DefaultValues.Message, 
            DefaultValues.CreationWithTime);
        var updatedEvent = new EventMessageModel(DefaultValues.EventMessageGuid, DefaultValues.Publisher, 
            DefaultValues.EventName, DefaultValues.CreationWithTime, DefaultValues.Message, 
            DefaultValues.ProcessedDate);
        await _messagesRepositorySql.UpdateEventMessageAsync(updatedEvent);
        var eventMesage = await _sQlDataBase.SelectAsync<dynamic>(
            @"Select * from SOLTECMQ.MQ.EventMessage where EventMessageGuid = @eventMessageGuid", 
            new
            {
                eventMessageGuid = DefaultValues.EventMessageGuid
            });
        ValidateEvent(updatedEvent, eventMesage.First());
    }

    [Test]
    public void update_an_event_message() 
    {
        GivenAnEventMessage(DefaultValues.EventMessageGuid, DefaultValues.EventName, DefaultValues.Message, 
            DefaultValues.CreationWithTime);
        var updatedEvent = new EventMessageModel(DefaultValues.EventMessageGuid, DefaultValues.Publisher, 
            DefaultValues.EventName, DefaultValues.CreationWithTime, DefaultValues.Message, 
            DefaultValues.ProcessedDate);
        _messagesRepositorySql.UpdateEventMessage(updatedEvent);
        var eventMesage = _sQlDataBase.Select<dynamic>(
            @"Select * from SOLTECMQ.MQ.EventMessage where EventMessageGuid = @eventMessageGuid", 
            new
            {
                eventMessageGuid = DefaultValues.EventMessageGuid
            });
        ValidateEvent(updatedEvent, eventMesage.First());
    }

    [Test]
    public void link_an_event_with_its_parent() 
    {
        var parentId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var model = new LinkEventModel(parentId, eventId);

        _messagesRepositorySql.LinkEvent(model);
        ValidateLinkEvent(model);
    }

    [Test]
    public async Task link_an_event_with_its_parent_async() 
    {
        var parentId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var model = new LinkEventModel(parentId, eventId);

        await _messagesRepositorySql.LinkEventAsync(model);
        ValidateLinkEvent(model);
    }

    private void ValidateLinkEvent(LinkEventModel expectedLinkEventModel) 
    {
        var linkedEvents = _sQlDataBase.Select<dynamic>(
            @"Select ParentId, EventId, Creation from SOLTECMQ.MQ.LinkedEvents where ParentId = @parentId", 
            new
            {
                parentId = expectedLinkEventModel.ParentId
            });
        linkedEvents.ForEach(l => ValidateLinkEventModel(l, expectedLinkEventModel));
    }

    private void ValidateLinkEventModel(dynamic linkedEvent, LinkEventModel expectedLinkEventModel) 
    {
        var model = new LinkEventModel(linkedEvent.ParentId, linkedEvent.EventId, linkedEvent.Creation);
        model.ParentId.Should().Be(expectedLinkEventModel.ParentId);
        model.EventId.Should().Be(expectedLinkEventModel.EventId);
    }

    private void ValidateEventMessage(EventMessageModel eventMessageDto, EventMessageModel expectedEventMessageModel) 
    {
        eventMessageDto.EventMessageGuid.Should().Be(expectedEventMessageModel.EventMessageGuid);
        eventMessageDto.EventName.Should().Be(expectedEventMessageModel.EventName);
        eventMessageDto.Message.Should().Be(expectedEventMessageModel.Message);
        eventMessageDto.CreationDate.Should().Be(expectedEventMessageModel.CreationDate);
        eventMessageDto.ProcessedDate.Should().Be(expectedEventMessageModel.ProcessedDate);
    }

    private static void ValidateEvent(EventMessageModel eventModel, dynamic eventMesage) 
    {
        eventModel.EventMessageGuid.ToString().Should().Be(eventMesage.EventMessageGuid.ToString());
        eventModel.EventName.ToString().Should().Be(eventMesage.EventName.ToString());
        eventModel.CreationDate.ToString().Should().Be(eventMesage.CreationDate.ToString());
        eventModel.Message.ToString().Should().Be(eventMesage.Message.ToString());
        eventModel.ProcessedDate.Should().Be(eventMesage.ProcessedDate);
        eventModel.Publisher.Should().Be(eventMesage.Publisher);
    }

    private void GivenAnEventMessage(Guid eventMessageGuid, string eventName, string message, string creationDate) 
    {
        _eventMessageBuilder.WithEventName(eventName)
                           .WithEventMessageGuid(eventMessageGuid)
                           .WithCreationDate(creationDate)
                           .WithMessage(message)
                           .Build();
    }

    private void GivenAnEventMessage() 
    {
        _eventMessageBuilder.Build();
    }

    private void GivenAExchange(string subscriberName) 
    {
        _exchangeBuilder.WithSubscriberName(subscriberName).Build();
    }

    private async Task ValidateUpdateAsync(MessageModel expectedUpdatedMessage) 
    {
        var messages = await _sQlDataBase.SelectAsync<dynamic>(
            @"Select * from SOLTECMQ.MQ.Message where MessageGuid = @messageGuid", 
            new { 
                messageGuid = expectedUpdatedMessage.MessageGuid 
            });
        ValidateUpdatedMessage(messages, expectedUpdatedMessage);
    }

    private void ValidateUpdate(MessageModel expectedUpdatedMessage) 
    {
        var messages = _sQlDataBase.Select<dynamic>(
            @"Select * from SOLTECMQ.MQ.Message where MessageGuid = @messageGuid", 
            new { 
                messageGuid = expectedUpdatedMessage.MessageGuid
                
            });
        ValidateUpdatedMessage(messages, expectedUpdatedMessage);
    }

    private void ValidateUpdatedMessage(IList<dynamic> messages, MessageModel expectedMessage) 
    {
        ((string)messages.First().ProcessedDate).Should().Be(expectedMessage.ProcessedDate);
        ((HttpStatusCode)messages.First().StatusCode).Should().Be(expectedMessage.StatusCode);
        ((string)messages.First().ErrorMessage).Should().Be(expectedMessage.ErrorMessage);
    }

    private void ValidateMessage(MessageModel expectedMessage, MessageModel messageModel) 
    {
        messageModel.MessageGuid.Should().Be(expectedMessage.MessageGuid);
        messageModel.EventName.Should().Be(expectedMessage.EventName);
        messageModel.EventMessageGuid.Should().Be(expectedMessage.EventMessageGuid);
        messageModel.Message.Should().Be(expectedMessage.Message);
        messageModel.ProcessedDate.Should().Be(expectedMessage.ProcessedDate);
        messageModel.StatusCode.Should().Be(expectedMessage.StatusCode);
        messageModel.ErrorMessage.Should().Be(expectedMessage.ErrorMessage);
    }

    private static MessageModel CreateAMessageModel() 
    {
        return new MessageModel(DefaultValues.EventMessageGuid, DefaultValues.EventName, DefaultValues.SubscriberName, 
            DefaultValues.Message);
    }

    private void GivenAMessage(Guid messageGuid, string subscriberName, string eventName, string processDate) 
    {
        _messageBuilder.WithMessageGuid(messageGuid)
                      .WithSubscriberName(subscriberName)
                      .WithEventName(eventName)
                      .WithProcessedDate(processDate)
                      .Build();
    }
}
