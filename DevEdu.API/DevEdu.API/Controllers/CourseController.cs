using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        public CourseController()
        {
        }

        //  api/Course/5
        [HttpGet("{id}")]
        public string GetCourse(int id)
        {
            return $"course №{id}";
        }

        //  api/Course
        [HttpGet]
        public string GetAllCourses()
        {
            return "All course";
        }

        //  api/course
        [HttpPost]
        public int AddCourse([FromBody] CourseInputModel model)
        {
            return 1;
        }

        //  api/course/5
        [HttpDelete("{id}")]
        public void DeleteCourse(int id)
        {

        }

        //  api/course/5
        [HttpPut("{id}")]
        public string UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            return $"Course №{id} change name to {model.Name} and description to {model.Description}";
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpPost("topic/{topicId}/tag/{tagId}")]
        public int AddTagToTopic(int topicId, int tagId)
        {
            return topicId;
        }

        //  api/course/topic/{topicId}/tag/{tagId}
        [HttpDelete("topic/{topicId}/tag/{tagId}")]
        public string DeleteTagAtTopic(int topicId, int tagId)
        {
            return $"deleted at topic with {topicId} Id tag with {tagId} Id";
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [HttpPost("{courseId}/material/{materialId}")]
        public string AddMaterialToCourse(int courseId, int materialId)
        {
            return $"Course {courseId} add  Material Id {materialId}";
        }

        //  api/course/{CourseId}/Material/{MaterialId}
        [HttpDelete("{courseId}/material/{materialId}")]
        public string RemoveMaterialFromCourse(int courseId, int materialId)
        {
            return $"Course {courseId} remove  Material Id:{materialId}";
        }

        //  api/course/{CourseId}/Task/{TaskId}
        [HttpPost("{courseId}/task/{taskId}")]
        public string AddTaskToCourse(int courseId, int taskId)
        {
            return $"Course {courseId} add  Task Id:{taskId}";
        }

        //  api/course/{CourseId}/Task/{TaskId}
        [HttpDelete("{courseId}/task/{taskId}")]
        public string RemoveTaskFromCourse(int courseId, int taskId)
        {
            return $"Course {courseId} remove  Task Id:{taskId}";
        }
    }
}