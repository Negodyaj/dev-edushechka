using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetAllNotificationByUserAsync(UserIdentityInfo userInfo);
        Task<NotificationDto> GetNotificationAsync(int id);
        Task<List<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<List<NotificationDto>> GetNotificationsByGroupIdAsync(int groupId);
        Task<List<NotificationDto>> GetNotificationsByRoleIdAsync(int RoleId);
        Task<NotificationDto> AddNotificationAsync(NotificationDto dto, UserIdentityInfo userInfo);
        Task DeleteNotificationAsync(int id, UserIdentityInfo userInfo);
        Task<NotificationDto> UpdateNotificationAsync(int id, NotificationDto dto, UserIdentityInfo userInfo);
    }
}