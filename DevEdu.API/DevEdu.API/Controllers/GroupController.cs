using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.InputModels;
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
        private readonly IGroupRepository _groupRepository;

        public GroupController(IMapper mapper, IGroupService service, IGroupRepository repository)
        {
            _mapper = mapper;
            _groupService = service;
            _groupRepository = repository;
        }

        //  api/Group/5
        [HttpGet("{id}")]
        [Description("Get Group")]
        public GroupDto GetGroupById(int id)
        {
            return _groupService.GetGroup(id);
        }

        //  api/Group/
        [HttpGet]
        [Description("Get all Groups")]
        public List<GroupInfoOutputModel> GetAllGroups()
        {
            var dto = _groupRepository.GetGroups();
            return _mapper.Map<List<GroupInfoOutputModel>>(dto);
        }

        //  api/Group
        [HttpPost]
        [Description("Create new Group")]
        public GroupDto AddGroup([FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            return _groupService.AddGroup(dto);
        }

        //  api/Group
        [HttpDelete("{id}")]
        [Description("Delete Group by Id")]
        public void DeleteGroup(int id)
        {
            _groupService.DeleteGroup(id);
        }

        //  api/Group
        [HttpPut]
        [Description("Update Group")]
        public GroupDto UpdateGroup(int id, [FromBody] GroupInputModel model)
        {
            var dto = _mapper.Map<GroupDto>(model);
            return _groupService.UpdateGroup(id, dto);
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
        public void AddUserToGroup(int groupId, int userId, int roleId) => _groupService.AddUserToGroup(groupId, userId, roleId);

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        public void DeleteUserFromGroup(int groupId, int userId) => _groupService.DeleteUserFromGroup(userId, groupId);
    }
}