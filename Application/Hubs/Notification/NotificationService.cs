using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs.Notification
{
    public class NotificationService : Hub
    {
        public void SendNotification(Domain.Entities.CrimeReporting.CrimeReporting message)
        {
            Clients.All.SendAsync("Thông báo : ", message);
        }
    }
}