using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface INotificationValidationHelper
    {
        Task<NotificationDto> GetNotificationByIdAndThrowIfNotFoundAsync(int notificationId);
        void CheckNotificationIsForGroup(NotificationDto dto, int userId);
        void CheckRoleIdUserIdGroupIdIsNotNull(NotificationDto dto);
    }
}