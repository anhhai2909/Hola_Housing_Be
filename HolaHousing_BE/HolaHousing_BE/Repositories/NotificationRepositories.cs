using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace HolaHousing_BE.Repositories
{
    public class NotificationRepositories : INotificationInterface
    {
        private readonly EXE201Context _context;
        public NotificationRepositories(EXE201Context context)
        {
            _context = context;
        }
        public int AddNotification(Notification n)
        {
            _context.Notifications.Add(n);
            _context.SaveChanges();
            return n.Id;
        }

        public bool DeleteNotification(int id)
        {
            Notification n = _context.Notifications.Find(id);
            if (n != null)
            {
                _context.Notifications.Remove(n);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Notification GetNotification(int id)
        {
            return _context.Notifications.Find(id);
        }

        public List<Notification> GetNotifications(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .ToList();
        }

        public bool UpdateNotification(Notification n)
        {
            _context.Notifications.Update(n);
            return _context.SaveChanges() > 0;
        }
    }
}
