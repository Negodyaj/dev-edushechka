using Microsoft.AspNetCore.Mvc;
using DevEdu.API.Models.InputModels;

namespace DevEdu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private GroupRepository _groupRepository;
        public GroupController()
        {
            _groupRepository = new GroupRepository();
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
            _groupRepository.AddGroupLesson(groupId,  lessonId);
        }

        // api/Group/{groupId}/lesson/{lessonId}
        [HttpDelete("{groupId}/lesson/{lessonId}")]
        public void RemoveGroupLessonReference(int groupId, int lessonId)
        {
            _groupRepository.RemoveGroupLesson(groupId, lessonId);
        }

    }
}