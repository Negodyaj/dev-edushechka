using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class NotificationValidationHelper : INotificationValidationHelper
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationValidationHelper(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDto> GetNotificationByIdAndThrowIfNotFoundAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetNotificationAsync(notificationId);
            if (notification == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(notification), notificationId));
            
            return notification;
        }

        public void CheckNotificationIsForGroup(NotificationDto dto, int userId)
        {
            if (dto.Group == null)
                throw new AuthorizationException(string.Format(ServiceMessages.AccessToNotificationDenied, userId));
        }

        public void CheckRoleIdUserIdGroupIdIsNotNull(NotificationDto dto)
        {
            if (dto.Role != null && dto.User != null
                 || dto.Role != null && dto.Group != null
                 || dto.User != null && dto.Group != null)
            {
                throw new Exception(string.Format(ServiceMessages.MoreOnePropertyHaveAValueMessage,
                    nameof(dto.Role), nameof(dto.Group), nameof(dto.User)));
            }
        }
    }
}