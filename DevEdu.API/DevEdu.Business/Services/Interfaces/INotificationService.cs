using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface INotificationService
    {
        NotificationDto GetNotification(int id);
        List<NotificationDto> GetNotificationsByUserId(int userId);
        List<NotificationDto> GetNotificationsByGroupId(int groupId);
        List<NotificationDto> GetNotificationsByRoleId(int RoleId);
        NotificationDto AddNotification(NotificationDto dto, UserIdentityInfo userInfo);
        void DeleteNotification(int id, UserIdentityInfo userInfo);
        NotificationDto UpdateNotification(int id, NotificationDto dto, UserIdentityInfo userInfo);
    }
}