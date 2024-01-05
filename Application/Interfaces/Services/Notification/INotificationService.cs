namespace Application.Interfaces.Services.Notification
{
    public interface INotificationService
    {
        void SendNotification(string user, string message);
    }
}