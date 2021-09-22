using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface INotificationRepository
    {
        Task<int> AddNotificationAsync(NotificationDto notificationDto);
        Task DeleteNotificationAsync(int id);
        Task<NotificationDto> GetNotificationAsync(int id);
        Task<List<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<List<NotificationDto>> GetNotificationsByGroupIdAsync(int groupId);
        Task<List<NotificationDto>> GetNotificationsByRoleIdAsync(int roleId);
        Task UpdateNotificationAsync(NotificationDto notificationDto);
    }
}