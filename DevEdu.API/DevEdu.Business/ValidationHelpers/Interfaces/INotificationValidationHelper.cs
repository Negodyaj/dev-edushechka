using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface INotificationValidationHelper
    {
        NotificationDto GetNotificationByIdAndThrowIfNotFound(int notificationId);
        void CheckTeacherAccessToNotificationForUpdateAndDelete(NotificationDto dto, int userId);
        void CheckNotificationIsForGroup(NotificationDto dto, int userId);
        void CheckRoleIdUserIdGroupIdIsNotNull(NotificationDto dto);
    }
}