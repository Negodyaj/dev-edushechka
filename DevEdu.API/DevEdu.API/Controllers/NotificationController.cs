using System.Collections.Generic;
using AutoMapper;
using System.ComponentModel;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Models.OutputModels;

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
        [Description("Return notification by id")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status200OK)]
        public NotificationInfoOutputModel GetNotification(int id)
        {
            var dto = _notificationService.GetNotification(id);
            var output = _mapper.Map<NotificationInfoOutputModel>(dto);
            return output;
        }

        //  api/notification/by-user/1
        [HttpGet("by-user/{userId}")]
        [Description("Return notifications by user")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        public List<NotificationInfoOutputModel> GetAllNotificationsByUserId(int userId)
        {
            var dto = _notificationService.GetNotificationsByUserId(userId);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notification/by-group/1
        [HttpGet("by-group/{groupId}")]
        [Description("Return notifications by group")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        public List<NotificationInfoOutputModel> GetAllNotificationsByGroupId(int groupId)
        {
            var dto = _notificationService.GetNotificationsByGroupId(groupId);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notification/by-role/1
        [HttpGet("by-role/{roleId}")]
        [Description("Return notifications by role")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        public List<NotificationInfoOutputModel> GetAllNotificationsByRoleId(int roleId)
        {
            var dto = _notificationService.GetNotificationsByRoleId(roleId);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notification
        [HttpPost]
        [Description("Add new notification")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status201Created)]
        public NotificationInfoOutputModel AddNotification([FromBody] NotificationAddInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            var output = _notificationService.AddNotification(dto);
            return _mapper.Map<NotificationInfoOutputModel>(output);
        }

        //  api/notification/5
        [HttpDelete("{id}")]
        [Description("Delete notification by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteNotification(int id)
        {
            _notificationService.DeleteNotification(id);
        }

        //  api/notification/5
        [HttpPut("{id}")]
        [Description("Update notification by id")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status200OK)]
        public NotificationInfoOutputModel UpdateNotification(int id, [FromBody] NotificationUpdateInputModel model)
        {
            var dto = _mapper.Map<NotificationDto>(model);
            var output = _notificationService.UpdateNotification(id, dto);
            return _mapper.Map<NotificationInfoOutputModel>(output);
        }
    }
}