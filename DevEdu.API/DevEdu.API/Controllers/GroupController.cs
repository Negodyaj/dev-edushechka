using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using DevEdu.DAL.Models;
using DevEdu.API.Models.OutputModels;
using System.Collections.Generic;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        //  api/Group
        [HttpPut]
        [Description("Update Group by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            _groupService.UpdateGroup(id, dto);
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        [Description("Change group status by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void ChangeGroupStatus(int groupId, int statusId)
        {

        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        [Description("Add group lesson reference")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddGroupLessonReference(int groupId, int lessonId)
        {
            _groupService.AddGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        [Description("Delete group lesson reference")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void RemoveGroupLessonReference(int groupId, int lessonId)
        {
            _groupService.RemoveGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpPost("{groupId}/material/{materialId}")]
        [Description("Add material to group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public int AddGroupMaterialReference(int groupId, int materialId)
        {
            return _groupService.AddGroupMaterialReference(groupId, materialId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpDelete("{groupId}/material/{materialId}")]
        [Description("Remove material from group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public int RemoveGroupMaterialReference(int groupId, int materialId)
        {
            return _groupService.RemoveGroupMaterialReference(groupId, materialId);
        }

        //  api/group/1/user/2/role/1
        [HttpPost("{groupId}/user/{userId}/role/{roleId}")]
        [Description("Add user to group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupService.AddUserToGroup(groupId, userId, roleId);

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        [Description("Delete user from group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteUserFromGroup(int groupId, int userId) => _groupService.DeleteUserFromGroup(userId, groupId);

        //  api/group/1/task/1
        [HttpGet("{groupId}/task/{taskId}")]
        [Description("Return task group by both id")]
        [ProducesResponseType(typeof(GroupTaskInfoFullOutputModel), StatusCodes.Status200OK)]
        public GroupTaskInfoFullOutputModel GetGroupTask(int groupId, int taskId)
        {
            var dto = _groupService.GetGroupTask(groupId, taskId);
            var output = _mapper.Map<GroupTaskInfoFullOutputModel>(dto);
            return output;
        }

        //  api/group/1/task/
        [HttpGet("{groupId}/tasks")]
        [Description("Get all tasks by group")]
        [ProducesResponseType(typeof(List<GroupTaskInfoWithTaskOutputModel>), StatusCodes.Status200OK)]
        public List<GroupTaskInfoWithTaskOutputModel> GetTasksByGroupId(int groupId)
        {
            var dto = _groupService.GetTasksByGroupId(groupId);
            var output = _mapper.Map<List<GroupTaskInfoWithTaskOutputModel>>(dto);
            return output;
        }

        //  api/group/1/task/1
        [HttpPost("{groupId}/task/{taskId}")]
        [Description("Add task to group")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddTaskToGroup(int groupId, int taskId, [FromBody] GroupTaskInputModel model)
        {
            var dto = _mapper.Map<GroupTaskDto>(model);
            return _groupService.AddTaskToGroup(groupId, taskId, dto);
        }

        //  api/group/1/task/1
        [HttpDelete("{groupId}/task/{taskId}")]
        [Description("Delete task from group")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTaskFromGroup(int groupId, int taskId)
        {
            _groupService.DeleteTaskFromGroup(groupId, taskId);
        }

        //  api/comment/5
        [HttpPut("{groupId}/task/{taskId}")]
        [Description("Update task by group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public GroupTaskInfoOutputModel UpdateGroupTask(int groupId, int taskId, [FromBody] GroupTaskInputModel model)
        {
            var dto = _mapper.Map<GroupTaskDto>(model);
            var output = _groupService.UpdateGroupTask(groupId, taskId, dto);
            return _mapper.Map<GroupTaskInfoOutputModel>(output);
        }
    }
}