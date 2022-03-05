using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public NotificationsController(IMapper mapper, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        //  api/notifications
        [HttpGet]
        [Description("Return notification by userInfo")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<NotificationInfoOutputModel>> GetAllNotificationByUserAsync()
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = await _notificationService.GetAllNotificationByUserAsync(userInfo);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notifications/by-group/1
        [HttpGet("by-group/{groupId}")]
        [Description("Return notifications by group")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<NotificationInfoOutputModel>> GetAllNotificationsByGroupIdAsync(int groupId)
        {
            var dto = await _notificationService.GetNotificationsByGroupIdAsync(groupId);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notifications/by-role/1
        [HttpGet("by-role/{roleId}")]
        [Description("Return notifications by role")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<NotificationInfoOutputModel>> GetAllNotificationsByRoleIdAsync(int roleId)
        {
            var dto = await _notificationService.GetNotificationsByRoleIdAsync(roleId);
            var output = _mapper.Map<List<NotificationInfoOutputModel>>(dto);
            return output;
        }

        //  api/notifications
        [AuthorizeRoles(Role.Teacher, Role.Manager)]
        [HttpPost]
        [Description("Add new notification")]
        [ProducesResponseType(typeof(NotificationInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<NotificationInfoOutputModel>> AddNotificationAsync([FromBody] NotificationAddInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<NotificationDto>(inputModel);
            var outputDto = await _notificationService.AddNotificationAsync(dto, userInfo);
            var result = _mapper.Map<NotificationInfoOutputModel>(outputDto);
            return Created(new Uri($"api/Notification/{result.Id}", UriKind.Relative), result);
        }

        //  api/notifications/5
        [AuthorizeRoles(Role.Teacher, Role.Manager)]
        [HttpDelete("{id}")]
        [Description("Delete notification by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteNotificationAsync(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            await _notificationService.DeleteNotificationAsync(id, userInfo);
            return NoContent();
        }

        //  api/notifications/5
        [AuthorizeRoles(Role.Teacher, Role.Manager)]
        [HttpPut("{id}")]
        [Description("Update notification by id")]
        [ProducesResponseType(typeof(List<NotificationInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<NotificationInfoOutputModel> UpdateNotificationAsync(int id, [FromBody] NotificationUpdateInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<NotificationDto>(inputModel);
            var output = await _notificationService.UpdateNotificationAsync(id, dto, userInfo);
            var model = _mapper.Map<NotificationInfoOutputModel>(output);
            return model;
        }
    }
}