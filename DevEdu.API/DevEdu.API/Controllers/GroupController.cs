using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Extensions;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
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
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;

        public GroupController(IMapper mapper, IGroupService service)
        {
            _mapper = mapper;
            _groupService = service;
        }

        //  api/Group/5
        [HttpGet("{id}")]
        [Description("Return Group by id")]
        [ProducesResponseType(typeof(GroupFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupFullOutputModel GetGroup(int id)
        {
            var dto = _groupService.GetGroup(id);
            return _mapper.Map<GroupFullOutputModel>(dto);
        }

        //  api/Group/
        [HttpGet]
        [Description("Get all Groups")]
        [ProducesResponseType(typeof(List<GroupOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<GroupOutputModel> GetAllGroups()
        {
            var dto = _groupService.GetGroups();
            return _mapper.Map<List<GroupOutputModel>>(dto);
        }

        //  api/Group
        [HttpPost]
        [Description("Add new Group")]
        [ProducesResponseType(typeof(GroupOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupOutputModel AddGroup([FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var result = _groupService.AddGroup(dto);
            return _mapper.Map<GroupOutputModel>(result);
        }

        //  api/Group
        [HttpDelete("{id}")]
        [Description("Delete Group by Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        //  api/Group
        [HttpPut]
        [Description("Update Group by id")]
        [ProducesResponseType(typeof(GroupInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupInfoOutputModel UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var output = _groupService.UpdateGroup(id, dto);
            return _mapper.Map<GroupInfoOutputModel>(output);
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        [Description("Change group status by id")]
        [ProducesResponseType(typeof(GroupOutputBaseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public GroupOutputBaseModel ChangeGroupStatus(int groupId, int statusId)
        {
            var output = _groupService.ChangeGroupStatus(groupId, statusId);
            return _mapper.Map<GroupOutputBaseModel>(output);
        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        [Description("Add group lesson reference")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public string AddGroupToLesson(int groupId, int lessonId)
        {
            _groupService.AddGroupToLesson(groupId, lessonId);
            return $"Group {groupId} add  Lesson Id:{lessonId}";
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        [Description("Delete lesson from groupId")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public string RemoveGroupFromLesson(int groupId, int lessonId)
        {
            _groupService.RemoveGroupFromLesson(groupId, lessonId);
            return $"Group {groupId} remove  Lesson Id:{lessonId}";
        }

        // api/Group/{groupId}/material/{materialId}
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpPost("{groupId}/material/{materialId}")]
        [Description("Add material to group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void AddGroupMaterialReference(int groupId, int materialId)
        {
            var userInfo = this.GetUserIdAndRoles();
            _groupService.AddGroupMaterialReference(groupId, materialId, userInfo);
        }

        // api/Group/{groupId}/material/{materialId}
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpDelete("{groupId}/material/{materialId}")]
        [Description("Remove material from group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void RemoveGroupMaterialReference(int groupId, int materialId)
        {
            var userInfo = this.GetUserIdAndRoles();
            _groupService.RemoveGroupMaterialReference(groupId, materialId, userInfo);
        }

        //  api/group/1/user/2/role/1
        [AuthorizeRoles(Role.Manager)]
        [HttpPost("{groupId}/user/{userId}/role/{roleId}")]
        [Description("Add user to group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult AddUserToGroup(int groupId, int userId, int roleId)
        {
            _groupService.AddUserToGroup(groupId, userId, roleId);
            return NoContent();
        }

        //  api/group/1/user/2
        [AuthorizeRoles(Role.Manager)]
        [HttpDelete("{groupId}/user/{userId}")]
        [Description("Delete user from group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteUserFromGroup(int groupId, int userId)
        {
            _groupService.DeleteUserFromGroup(userId, groupId);
            return NoContent();
        }
    }
}