using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface INotificationService
    {
        NotificationDto GetNotification(int id);
        List<NotificationDto> GetNotificationsByUserId(int userId);
        List<NotificationDto> GetNotificationsByGroupId(int groupId);
        List<NotificationDto> GetNotificationsByRoleId(int RoleId);
        int AddNotification(NotificationDto dto);
        void DeleteNotification(int id);
        void UpdateNotification(int id, NotificationDto dto);
    }
}