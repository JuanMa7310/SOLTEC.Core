using SOLTEC.Core.DTOS;

namespace SOLTEC.Core.NotificationMessage.Interfaces;

public interface INotificationMessage 
{
    Task SendNotification(NotificationMessageDto message);
}
