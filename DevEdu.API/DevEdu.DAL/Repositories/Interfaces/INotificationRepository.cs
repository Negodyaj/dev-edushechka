using System.Collections.Generic;
using DevEdu.DAL.Models;


namespace DevEdu.DAL.Repositories
{
    public interface INotificationRepository
    {
        int AddNotification(NotificationDto notificationDto);

        void DeleteNotification(int id);

        NotificationDto GetNotification(int id);

        List<NotificationDto> GetNotificationsByUser(int userId);

        void UpdateNotification(NotificationDto notificationDto);
    }
}
