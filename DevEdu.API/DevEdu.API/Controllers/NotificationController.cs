using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public NotificationController(IMapper mapper, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        //  api/notification/5
        [HttpGet("{id}")]
        public NotificationDto GetNotification(int id)
        {
            return _notificationService.GetNotification(id);
        }

        //  api/notification/by-user/1
        [HttpGet("by-user/{userId}")]
        public List<NotificationDto> GetAllNotificationsByUserId(int userId)
        {
            return _notificationService.GetNotificationsByUserId(userId);
        }

        //  api/notification
        [HttpPost]
        public int AddNotification([FromBody] NotificationAddInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            return _notificationService.AddNotification(dto);
        }

        //  api/notification/5
        [HttpDelete("{id}")]
        public void DeleteNotification(int id)
        {
            _notificationService.DeleteNotification(id);
        }

        //  api/notification/5
        [HttpPut("{id}")]
        public string UpdateNotification(int id, [FromBody] NotificationUpdateInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            _notificationService.UpdateNotification(id,dto);
            return $"Text notification №{id} change to {model.Text}";
        }
    }
}

