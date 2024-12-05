using SOLTEC.Core.MessageMQ.Commands;
using SOLTEC.Core.MessageMQ.Responses;
using System.Net;

namespace SOLTEC.Core.MessageMQ.Interfaces;

public interface IMessage 
{
    Task<Guid> PushAsync(MessageCommand messageCommand);
    Guid Push(MessageCommand messageCommand);
    Task LinkEventAsync(LinkedEventCommand command);
    void LinkEvent(LinkedEventCommand command);
    Task<List<MessageResponse>> PopAsync(string subscriber, string eventName);
    List<MessageResponse> Pop(string subscriber, string eventName);
    Task ExchangeProcessAsync();
    void ExchangeProcess();
    Task ChangeStatusProcessingAsync(Guid messageId, HttpStatusCode statusCode, string errorMessage);
    void ChangeStatusProcessing(Guid messageId, HttpStatusCode statusCode, string errorMessage);
    void Dispose();
}
