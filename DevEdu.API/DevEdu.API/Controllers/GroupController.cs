using System.Collections.Generic;
using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupService _groupService;
        private readonly IGroupRepository _groupRepository;

        public GroupController(IMapper mapper, IGroupService groupService, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _groupService = groupService;
            _groupRepository = groupRepository;
        }

        //  api/Group/5
        [HttpGet("{id}")]
        public string GetGroupById(int id)
        {
            return $"Group №{id}";
        }

        //  api/Group/
        [HttpGet]
        public string GetAllGroups()
        {
            return "All Groups";
        }

        //  api/Group
        [HttpPost]
        public int AddGroup([FromBody] GroupInputModel model)
        {
            return 1;
        }

        //  api/Group
        [HttpDelete("{id}")]
        public void DeleteGroup(int id)
        {

        }

        //  api/Group
        [HttpPut]
        public string UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            return $"Group №{id} change courseId to {model.CourseId} and timetable to {model.Timetable} and startDate to {model.StartDate}" +
                   $"and paymentPerMonth {model.PaymentPerMonth}";
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        public void ChangeGroupStatus(int groupId, int statusId)
        {

        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        public void AddGroupLessonReference(int groupId, int lessonId)
        {
            _groupRepository.AddGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        public void RemoveGroupLessonReference(int groupId, int lessonId)
        {
            _groupRepository.RemoveGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpPost("{groupId}/material/{materialId}")]
        [Description("Add material to groop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public int AddGroupMaterialReference(int groupId, int materialId)
        {
            return _groupService.AddGroupMaterialReference(groupId, materialId);
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpDelete("{groupId}/material/{materialId}")]
        [Description("Remove material from groop")]
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