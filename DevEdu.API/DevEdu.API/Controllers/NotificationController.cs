using AutoMapper;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        //  api/notification/{id}
        [HttpGet("{id}")]
        [Description("Return notification by id")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public NotificationInfoOutputModel GetNotification(int id)
        {
            var dto = _notificationService.GetNotification(id);
            return _mapper.Map<NotificationInfoOutputModel>(dto);
        }

        //  api/notification/by-user/1
        [HttpGet("by-user/{userId}")]
        [Description("Return notifications by user")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<NotificationInfoOutputModel> GetAllNotificationsByUserId(int userId)
        {
            var list = _notificationService.GetNotificationsByUserId(userId);
            return _mapper.Map<List<NotificationInfoOutputModel>>(list);
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
            return _mapper.Map<List<NotificationInfoOutputModel>>(dto);
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
            return _mapper.Map<List<NotificationInfoOutputModel>>(dto);
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
            var outputDto = _notificationService.AddNotification(dto);
            var result = _mapper.Map<NotificationInfoOutputModel>(outputDto);
            return Created(new Uri($"api/Notification/{result.Id}", UriKind.Relative), result);
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
            return _mapper.Map<NotificationInfoOutputModel>(output);
        }
    }
}