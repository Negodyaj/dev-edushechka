using System.Collections.Generic;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public NotificationController(IMapper mapper, INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        //  api/notification/5
        [HttpGet("{id}")]
        public NotificationDto GetNotification(int id)
        {
            return _notificationRepository.GetNotification(id);
        }

        //  api/notification/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<NotificationDto> GetAllNotificationsByUserId(int userId)
        {
            return _notificationRepository.GetNotificationsByUserId(userId);
        }

        //  api/notification
        [HttpPost]
        public int AddNotification([FromBody] NotificationAddInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            return _notificationRepository.AddNotification(dto);
        }

        //  api/notification/5
        [HttpDelete("{id}")]
        public void DeleteNotification(int id)
        {
            _notificationRepository.DeleteNotification(id);
        }

        //  api/notification/5
        [HttpPut("{id}")]
        public string UpdateNotification(int id, [FromBody] NotificationUpdateInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            dto.Id = id;
            _notificationRepository.UpdateNotification(dto);
            return $"Text notification №{id} change to {model.Text}";
        }
    }
}

