using System.Collections.Generic;
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

        public int AddNotification( NotificationDto dto)
        {
            if(dto.Role!=0 && dto.User?.Id!=0)
            {
                throw new System.Exception("Only one property (RoleId, UserId or GroupId) should have a value");
            }
           return  _notificationRepository.AddNotification(dto);
        }

        public void DeleteNotification(int id) => _notificationRepository.DeleteNotification(id);

        public void UpdateNotification(int id, NotificationDto dto)
        {
            dto.Id = id;
            _notificationRepository.UpdateNotification(dto);
        }

    }
}
