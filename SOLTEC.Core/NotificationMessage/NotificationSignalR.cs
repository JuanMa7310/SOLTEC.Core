using SOLTEC.Core.DTOS;
using SOLTEC.Core.NotificationMessage.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace SOLTEC.Core.NotificationMessage;

public class NotificationSignalR : INotificationMessage 
{
    private string _connectionId;
    private string _channel;
    private IHubContext<MassiveActionHub> _hubContext;

    public NotificationSignalR(string connectionId, string channel, IHubContext<MassiveActionHub> hubContext) 
    {
        _connectionId = connectionId;
        _channel = channel;
        _hubContext = hubContext;
    }
    
    public virtual async Task SendNotification(NotificationMessageDto message) 
    {
        await _hubContext.Clients.Client(_connectionId).SendAsync(_channel, message);
    }
}
