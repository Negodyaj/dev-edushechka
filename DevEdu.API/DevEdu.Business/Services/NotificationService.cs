﻿using System.Collections.Generic;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public  class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public NotificationDto GetNotification(int id) => _notificationRepository.GetNotification(id);

        public List<NotificationDto> GetNotificationsByUserId(int userId) => _notificationRepository.GetNotificationsByUserId(userId);
        public List<NotificationDto> GetNotificationsByGroupId(int groupId) => _notificationRepository.GetNotificationsByGroupId(groupId);
        public List<NotificationDto> GetNotificationsByRoleId(int RoleId) =>    _notificationRepository.GetNotificationsByRoleId(RoleId);

        public int AddNotification(NotificationDto dto)
        {
            if (dto.Role != null && dto.User != null
                 || dto.Role != null && dto.Group != null
                 || dto.User != null && dto.Group != null)
            {
                throw new System.Exception("Only one property (RoleId, UserId or GroupId) should have a value");
            }
           return  _notificationRepository.AddNotification(dto);
        }

        public void DeleteNotification(int id) => _notificationRepository.DeleteNotification(id);

        public NotificationDto UpdateNotification(int id, NotificationDto dto)
        {
            dto.Id = id;
            _notificationRepository.UpdateNotification(dto);
            return _notificationRepository.GetNotification(id);
        }
    }
}
