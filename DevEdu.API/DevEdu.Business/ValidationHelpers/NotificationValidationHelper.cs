using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class NotificationValidationHelper : INotificationValidationHelper
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationValidationHelper(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void CheckNotificationExistence(int notificationId)
        {
            var notification = _notificationRepository.GetNotification(notificationId);
            if (notification == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(notification), notificationId));
        }
    }
}