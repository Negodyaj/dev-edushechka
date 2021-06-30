using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {

        public TopicController()
        {

        }

        //  api/Course/5
        [HttpGet("{id}")]
        public string GetTopicById(int id)
        {
            return $"{id}";
        }

        [HttpGet("all")]
        public string GetAllTopic()
        {
            return "All topic";
        }

        //  api/course
        [HttpPost]
        public int AddTopic([FromBody] TopicInputModel model)
        {
            return 1;
        }

        //  api/course
        [HttpDelete]
        public string DeleteTopic(int id)
        {
            return "Delete topic";
        }

        //  api/course
        [HttpPut]
        public string UpdateTopic(int id, string name, string duration)
        {
            return "Update topic";
        }

        

    }
}
