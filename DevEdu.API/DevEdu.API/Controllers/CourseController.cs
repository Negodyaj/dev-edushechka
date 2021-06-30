using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //  api/Course/all
        [HttpGet("all")]
        public string GetAllCourse()
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
        public string UpdateCourse(int id, CourseInputModel model)
        {
            return $"Course №{id} change name to {model.Name} and description to {model.Description}";
        }

        [HttpPost("topic/{topicId}/tag/{tagId}")]
        public int AddTagToTopic(int topicId, int tagId)
        {
            return topicId;
        }

        [HttpDelete("topic/{topicId}/tag/{tagId}")]
        public string DeleteTagAtTopic(int topicId, int tagId)
        {
            return $"deleted at topic with {topicId} Id tag with {tagId} Id";
        }

        [HttpPost("Course/{CourseId}/Material/{MaterialId}")]
        public string AddMaterialToCourse(int CourseId, int MaterialId)
        {
            return $"Course {CourseId} add  Material Id {MaterialId}";
        }

        [HttpDelete("Course/{CourseId}/Material/{MaterialId}")]
        public string RemoveMaterialToCourse(int CourseId, int MaterialId)
        {
            return $"Course {CourseId} remove  Material Id:{MaterialId}";
        }

        [HttpPost("Course/{CourseId}/Task/{TaskId}")]
        public string AddTaskToCourse(int CourseId, int TaskId)
        {
            return $"Course {CourseId} add  Task Id:{TaskId}";
        }

        [HttpDelete("Course/{CourseId}/Task/{TaskId}")]
        public string RemoveTaskToCourse(int CourseId, int TaskId)
        {
            return $"Course {CourseId} remove  Task Id:{TaskId}";
        }
    }
}