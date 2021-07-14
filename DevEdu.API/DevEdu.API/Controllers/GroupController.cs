using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Services;
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
        [ProducesResponseType(typeof(CommentInfoOutputModel), StatusCodes.Status200OK)]
        public GroupInfoOutputModel GetGroup(int id)
        {
            var dto = _groupService.GetGroup(id);
            return _mapper.Map<GroupInfoOutputModel>(dto);           
        }

        //  api/Group/
        [HttpGet]
        [Description("Get all Groups")]
        [ProducesResponseType(typeof(List<GroupInfoOutputModel>), StatusCodes.Status200OK)]
        public List<GroupInfoOutputModel> GetAllGroups()
        {
            var dto = _groupService.GetGroups();
            return _mapper.Map<List<GroupInfoOutputModel>>(dto);
        }

        //  api/Group
        [HttpPost]
        [Description("Add new Group")]
        [ProducesResponseType(typeof(GroupInfoOutputModel), StatusCodes.Status201Created)]
        public GroupInfoOutputModel AddGroup([FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var result = _groupService.AddGroup(dto);
            return _mapper.Map<GroupInfoOutputModel>(result);
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
        public GroupInfoOutputModel UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            var result = _groupService.UpdateGroup(id, dto);
            return _mapper.Map<GroupInfoOutputModel>(result);
        }

        //  api/Group/{groupId}/change-status/{statusId}
        [HttpPut("{groupId}/change-status/{statusId}")]
        [Description("Change group status by id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void ChangeGroupStatus(int groupId, int statusId)
        {

        }

        //add group_lesson relation
        // api/Group/{groupId}/lesson/{lessonId}
        [HttpPost("{groupId}/lesson/{lessonId}")]
        [Description("Add group lesson reference")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddGroupLessonReference(int groupId, int lessonId)
        {
            _groupService.AddGroupLesson(groupId, lessonId);
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        [Description("Delete group lesson reference")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupService.AddUserToGroup(groupId, userId, roleId);

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        [Description("Remove user from group")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void DeleteUserFromGroup(int groupId, int userId) => _groupService.DeleteUserFromGroup(userId, groupId);
    }
}