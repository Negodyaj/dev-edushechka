using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface INotificationRepository
    {
        public int AddNotification(NotificationDto notificationDto);
        public void DeleteNotification(int id);
        public NotificationDto GetNotification(int id);
        public List<NotificationDto> GetNotificationsByUserId(int userId);
        public void UpdateNotification(NotificationDto notificationDto);
    }
}
