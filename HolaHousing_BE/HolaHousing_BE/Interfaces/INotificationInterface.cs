using HolaHousing_BE.Models;

namespace HolaHousing_BE.Interfaces
{
    public interface INotificationInterface
    {
        Notification GetNotification(int id);
        List<Notification> GetNotifications(int userId);
        int AddNotification(Notification n);
        bool UpdateNotification(Notification n);
        bool DeleteNotification(int id);

    }
}
