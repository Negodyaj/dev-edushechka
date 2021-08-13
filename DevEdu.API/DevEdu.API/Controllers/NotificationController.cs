using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<NotificationInfoOutputModel> AddNotification([FromBody] NotificationAddInputModel inputModel)
        {
            var dto = _mapper.Map<NotificationDto>(inputModel);
            var output = _notificationService.AddNotification(dto);
            var model = _mapper.Map<NotificationInfoOutputModel>(output);
            return StatusCode(201,model);
        }

        //  api/notification/5
        [HttpDelete("{id}")]
        [Description("Delete notification by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteNotification(int id)
        {
            _notificationService.DeleteNotification(id);
            return NoContent();
        }

        //  api/notification/5
        [HttpPut("{id}")]
        [Description("Update notification by id")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public NotificationInfoOutputModel UpdateNotification(int id, [FromBody] NotificationUpdateInputModel inputModel)
        {
            var dto = _mapper.Map<NotificationDto>(inputModel);
            var output = _notificationService.UpdateNotification(id, dto);
            var model = _mapper.Map<NotificationInfoOutputModel>(output);
            return model;
        }
    }
}