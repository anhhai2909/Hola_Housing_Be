using AutoMapper;
using HolaHousing_BE.DTO;
using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HolaHousing_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationInterface _notificationInterface;
        private readonly NotificationService _notificationService;

        public NotificationsController(INotificationInterface notificationInterface, IHubContext<NotificationHub> hubContext)
        {
            _notificationInterface = notificationInterface;
            _notificationService = new NotificationService(hubContext);
        }

        [HttpGet]
        public IActionResult GetNotification([FromQuery] int userId, [FromQuery] int top)
        {
            var notifications = _notificationInterface.GetNotifications(userId);
            IEnumerable<Notification> sortedNotifications = notifications.OrderByDescending(n => n.CreatedDate);
            if (top > 0)
            {
                sortedNotifications = sortedNotifications.Take(top);
            }
            return Ok(sortedNotifications.ToList());
        }

        [HttpPost("mark/{id}")]
        public IActionResult MarkReadedNotification(int id)
        {
            var notification = _notificationInterface.GetNotification(id);

            if (notification == null)
            {
                return NotFound($"Notification with id {id} not found.");
            }
            notification.IsRead = true;
            _notificationInterface.UpdateNotification(notification);
            return Ok($"Notification with id {id} marked as read.");
        }

    }
}
