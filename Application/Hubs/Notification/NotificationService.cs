using Application.Features.CrimeReporting.Command.Add;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs.Notification
{
    public class NotificationService : Hub
    {
        private static readonly Dictionary<string, List<AddCrimeReportingCommand>> OfflineNotifications = new Dictionary<string, List<AddCrimeReportingCommand>>();

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            // Lưu trữ thông báo khi client offline
            if (!OfflineNotifications.ContainsKey(userId))
            {
                OfflineNotifications[userId] = new List<AddCrimeReportingCommand>();
            }
            // ... Lưu trữ các thông báo vào danh sách OfflineNotifications[Context.ConnectionId]

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(AddCrimeReportingCommand message)
        {
            if (Context.ConnectionId != null && Clients.Client(Context.ConnectionId) != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", message);
            }
            else
            {
                var httpContext = Context.GetHttpContext();
                var userId = httpContext.Request.Query["userId"].ToString();
                // Lưu trữ thông báo khi client offline
                if (!OfflineNotifications.ContainsKey(userId))
                {
                    OfflineNotifications[userId] = new List<AddCrimeReportingCommand>();
                }
                OfflineNotifications[userId].Add(message);
            }
            // await Clients.All.SendAsync("ReceiveNotification", message);
        }

        // Phương thức để gửi thông báo lưu trữ khi client online
        public async Task SendOfflineNotifications()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();
            if (OfflineNotifications.ContainsKey(userId))
            {
                var offlineMessages = OfflineNotifications[userId];
                foreach (var message in offlineMessages)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification", message);
                }
                OfflineNotifications.Remove(userId);
            }
        }
        // public Task SendNotification(AddCrimeReportingCommand message)
        // {
        //     return Clients.All.SendAsync("ReceiveNotification", message);
        // }
    }
}