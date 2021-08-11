using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.DAL.Enums;
using System;

namespace DevEdu.Business.ValidationHelpers
{
    public class NotificationValidationHelper : INotificationValidationHelper
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserValidationHelper _userValidationHelper;

        public NotificationValidationHelper(INotificationRepository notificationRepository, IUserValidationHelper userValidationHelper)
        {
            _notificationRepository = notificationRepository;
            _userValidationHelper = userValidationHelper;
        }

        public NotificationDto GetNotificationByIdAndThrowIfNotFound(int notificationId)
        {
            var notification = _notificationRepository.GetNotification(notificationId);
            if (notification == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(notification), notificationId));
            return notification;
        }

        public void CheckTeacherAccessToNotificationForUpdateAndDelete(NotificationDto dto, int userId)
        {
            _userValidationHelper.CheckAuthorizationUserToGroup(dto.Group.Id, userId, Role.Teacher);
        }
        public void CheckNotificationIsForGroup(NotificationDto dto, int userId)
        {
            if(dto.Group == null)
                throw new AuthorizationException(string.Format(ServiceMessages.AccessToNotificationDenied, userId, dto.Id));
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