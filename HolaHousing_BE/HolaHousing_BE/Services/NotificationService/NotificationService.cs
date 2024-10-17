using HolaHousing_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HolaHousing_BE.Services.NotificationService
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async void SendNotification(Notification n, int userId)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", n, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }
    }
}
