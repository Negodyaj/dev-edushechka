using AutoMapper;
using DevEdu.API.Common;
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

namespace DevEdu.API.Controllers
{
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
        [ProducesResponseType(typeof(GroupFullOutputModel), StatusCodes.Status200OK)]  // todo
        public GroupFullOutputModel GetGroup(int id)
        {
            var dto = _groupService.GetGroup(id);
            return _mapper.Map<GroupFullOutputModel>(dto);
        }

        //  api/Group/
        [HttpGet]
        [Description("Get all Groups")]
        [ProducesResponseType(typeof(List<GroupOutputModel>), StatusCodes.Status200OK)]
        public List<GroupOutputModel> GetAllGroups()
        {
            var dto = _groupService.GetGroups();
            return _mapper.Map<List<GroupOutputModel>>(dto);
        }

        //  api/Group
        [HttpPost]
        [Description("Add new Group")]
        [ProducesResponseType(typeof(GroupOutputModel), StatusCodes.Status201Created)]
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
        public void DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        //  api/Group
        [HttpPut]
        [Description("Update Group by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public GroupInfoOutputModel UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var output = _groupService.UpdateGroup(id, dto);
            return _mapper.Map<GroupInfoOutputModel>(output);
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        [Description("Change group status by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public GroupOutputBaseModel ChangeGroupStatus(int groupId, int statusId)
        {
            var output = _groupService.ChangeGroupStatus(groupId, statusId);
            return _mapper.Map<GroupOutputBaseModel>(output);
        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        [Description("Add group lesson reference")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public string AddGroupToLesson(int groupId, int lessonId)
        {
            _groupService.AddGroupToLesson(groupId, lessonId);
            return $"Group {groupId} add  Lesson Id:{lessonId}";
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        [Description("Delete lesson from groupId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        [HttpPost("{groupId}/user/{userId}/role/{roleId}")]
        [Description("Add user to group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoles(Role.Manager)]
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupService.AddUserToGroup(groupId, userId, roleId);

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        [Description("Delete user from group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoles(Role.Manager)]
        public void DeleteUserFromGroup(int groupId, int userId) => _groupService.DeleteUserFromGroup(userId, groupId);
    }
}