﻿using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
using System.Security.Claims;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private ClaimsIdentity _claimsIdentity;

        public GroupController(IMapper mapper, IGroupService service)
        {
            _claimsIdentity = this.User.Identity as ClaimsIdentity;
            _mapper = mapper;
            _groupService = service;
            //var role = _claimsIdentity.FindAll(ClaimsIdentity.DefaultRoleClaimType)?.ToList();  Над проверить, но по идеи получу список ролей
        }

        //  api/Group
        [HttpPost]
        [Description("Add new Group")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(GroupOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupOutputModel AddGroup([FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var result = _groupService.AddGroup(dto);
            return _mapper.Map<GroupOutputModel>(result);
        }

        //  api/Group/5
        [HttpGet("{id}")]
        [Description("Return Group by id")]
        [AuthorizeRoles()]
        [ProducesResponseType(typeof(GroupFullOutputModel), StatusCodes.Status200OK)]  // todo
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupFullOutputModel GetGroup(int id)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);
            
            var dto = _groupService.GetGroup(id, userId);
            return _mapper.Map<GroupFullOutputModel>(dto);
        }

        //  api/Group/
        [HttpGet]
        [Description("Get all Groups")]
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [ProducesResponseType(typeof(List<GroupOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public List<GroupOutputModel> GetAllGroups()
        {
            var dto = _groupService.GetGroups();
            return _mapper.Map<List<GroupOutputModel>>(dto);
        }


        //  api/Group
        [HttpDelete("{id}")]
        [Description("Delete Group by Id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public void DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        //  api/Group/{Id}
        [HttpPut("{id}")]
        [Description("Update Group by id")]
        [AuthorizeRoles(Role.Manager, Role.Teacher)]
        [ProducesResponseType(typeof(GroupInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupInfoOutputModel UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            var dto = _mapper.Map<GroupDto>(model);
            var output = _groupService.UpdateGroup(id, dto, userId);
            return _mapper.Map<GroupInfoOutputModel>(output);
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        [Description("Change group status by id")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(GroupOutputBaseModel), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupOutputBaseModel ChangeGroupStatus(int groupId, GroupStatus statusId)
        {
            var output = _groupService.ChangeGroupStatus(groupId, statusId);
            return _mapper.Map<GroupOutputBaseModel>(output);
        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        [Description("Add group lesson reference")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public string AddGroupToLesson(int groupId, int lessonId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            _groupService.AddGroupToLesson(groupId, lessonId, userId);
            return $"Group {groupId} add  Lesson Id:{lessonId}";
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        [Description("Delete lesson from groupId")]
        [AuthorizeRoles(Role.Manager)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public string RemoveGroupFromLesson(int groupId, int lessonId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            _groupService.RemoveGroupFromLesson(groupId, lessonId, userId);
            return $"Group {groupId} remove  Lesson Id:{lessonId}";
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpPost("{groupId}/material/{materialId}")]
        [Description("Add material to group")]
        [AuthorizeRoles(Role.Manager, Role.Teacher, Role.Tutor)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public int AddGroupMaterialReference(int groupId, int materialId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            return _groupService.AddGroupMaterialReference(groupId, materialId, userId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpDelete("{groupId}/material/{materialId}")]
        [Description("Remove material from group")]
        [AuthorizeRoles(Role.Manager, Role.Teacher, Role.Tutor)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public int RemoveGroupMaterialReference(int groupId, int materialId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            return _groupService.RemoveGroupMaterialReference(groupId, materialId, userId);
        }

        //  api/group/1/user/2/role/1
        [HttpPost("{groupId}/user/{userId}/role/{roleId}")]
        [Description("Add user to group")]
        [AuthorizeRoles(Role.Manager, Role.Teacher)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public void AddUserToGroup(int groupId, int userId, Role roleId)
        {
            var currentUserId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            _groupService.AddUserToGroup(groupId, userId, roleId, currentUserId);
        }

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        [Description("Delete user from group")]
        [AuthorizeRoles(Role.Manager, Role.Teacher)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public void DeleteUserFromGroup(int userId, int groupId)
        {
            var currentUserId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            _groupService.DeleteUserFromGroup(userId, groupId, currentUserId);
        }

        //  api/group/1/task/1
        [HttpGet("{groupId}/task/{taskId}")]
        [Description("Return task group by both id")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [ProducesResponseType(typeof(GroupTaskInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupTaskInfoFullOutputModel GetGroupTask(int groupId, int taskId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            var dto = _groupService.GetGroupTask(groupId, taskId, userId);
            var output = _mapper.Map<GroupTaskInfoFullOutputModel>(dto);
            return output;
        }

        //  api/group/1/task/
        [HttpGet("{groupId}/tasks")]
        [Description("Get all tasks by group")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [ProducesResponseType(typeof(List<GroupTaskInfoWithTaskOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public List<GroupTaskInfoWithTaskOutputModel> GetTasksByGroupId(int groupId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            var dto = _groupService.GetTasksByGroupId(groupId, userId);
            var output = _mapper.Map<List<GroupTaskInfoWithTaskOutputModel>>(dto);
            return output;
        }

        //  api/group/1/task/1
        [HttpPost("{groupId}/task/{taskId}")]
        [Description("Add task to group")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public int AddTaskToGroup(int groupId, int taskId, [FromBody] GroupTaskInputModel model)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            var dto = _mapper.Map<GroupTaskDto>(model);
            return _groupService.AddTaskToGroup(groupId, taskId, dto, userId);
        }

        //  api/group/1/task/1
        [HttpDelete("{groupId}/task/{taskId}")]
        [Description("Delete task from group")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public void DeleteTaskFromGroup(int groupId, int taskId)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            _groupService.DeleteTaskFromGroup(groupId, taskId, userId);
        }

        //  api/comment/5
        [HttpPut("{groupId}/task/{taskId}")]
        [Description("Update task by group")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [ProducesResponseType(typeof(GroupTaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public GroupTaskInfoOutputModel UpdateGroupTask(int groupId, int taskId, [FromBody] GroupTaskInputModel model)
        {
            var userId = Convert.ToInt32(_claimsIdentity.FindFirst(JwtRegisteredClaimNames.NameId)?.Value);

            var dto = _mapper.Map<GroupTaskDto>(model);
            var output = _groupService.UpdateGroupTask(groupId, taskId, dto, userId);
            return _mapper.Map<GroupTaskInfoOutputModel>(output);
        }
    }
}