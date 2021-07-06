using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Repositories;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;
        public GroupController(IMapper mapper, IGroupRepository groupRepository)
        {
            _mapper = mapper;
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
        public string AddGroupLessonReference(int groupId, int lessonId)
        {
            return (lessonId + groupId).ToString();
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        public string RemoveGroupLessonReference(int groupId, int lessonId)
        {
            return (lessonId - groupId).ToString();
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpPost("{groupId}/material/{materialId}")]
        public string AddGroupMaterialReference(int materialId, int groupId)
        {
            _groupRepository.AddGroupMaterialReference(materialId,groupId);
            return $"Material №{materialId} add to group {groupId}";
        }

        // api/Group/{groupId}/material/{materialId}
        [HttpDelete("{groupId}/material/{materialId}")]
        public string RemoveGroupMaterialReference(int materialId, int groupId)
        {
            _groupRepository.RemoveGroupMaterialReference(materialId,groupId);
            return $"Material №{materialId} remove from group {groupId}";
        }

        //  api/group/1/user/2/role/1
        [HttpPost("{groupId}/user/{userId}/role/{roleId}")]
        public void AddUserToGroup(int groupId, int userId, int roleId)
        {
            _groupRepository.AddUserToGroup(groupId, userId, roleId);
        }

        //  api/group/1/user/2
        [HttpDelete("{groupId}/user/{userId}")]
        public void DeleteUser_Group(int groupId, int userId)
        {
            _groupRepository.DeleteUserFromGroup(userId, groupId);
        }
    }
}