using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        MappersController mappersController = new MappersController();
        CourseRepository courseRepository = new CourseRepository();
        public CourseController()
        {
        }

        //  api/Course/5
        [HttpGet("{id}")]
        public CourseDto GetCourse(int id)
        {
            return courseRepository.GetCourse(id);
        }

        //  api/Course
        [HttpGet]
        public List<CourseDto> GetAllCourses()
        {
            return courseRepository.GetCourses();
        }

        //  api/course
        [HttpPost]
        public int AddCourse([FromBody] CourseInputModel model)
        {
            return courseRepository.AddCourse(mappersController.MapCourseModelToCourseDto(model));
        }

        //  api/course/5
        [HttpDelete("{id}")]
        public void DeleteCourse(int id)
        {
            courseRepository.DeleteCourse(id);
        }

        //  api/course/5
        [HttpPut("{id}")]
        public string UpdateCourse(int id, [FromBody] CourseInputModel model)
        {
            courseRepository.UpdateCourse(id,(mappersController.MapCourseModelToCourseDto(model)));
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