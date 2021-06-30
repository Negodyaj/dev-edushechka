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

        //  api/course
        [HttpDelete]
        public void DeleteCourse(int id)
        {

        }

        //  api/course
        [HttpPut]
        public string UpdateCourse(int id, string name, string description)
        {
            return $"Course №{id} change name to {name} and description to {description}";
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
    }
}