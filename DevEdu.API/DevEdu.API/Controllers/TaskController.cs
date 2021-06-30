using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevEdu.API.Models.InputModels;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        public TaskController()
        {

        }

        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {

            return 1;
        }

        [HttpDelete("{taskId}")]
        public string DeleteTask(int taskId)
        {
            return $"deleted task with {taskId} Id";
        }

        [HttpPut("{taskId}")]
        public string UpdateTask([FromBody] TaskInputModel model, int taskId)
        {

            return $"update task with {taskId} Id";
        }

        [HttpPost]

        public int AddTagTask(int tagId, int taskId)
        {
            return 1;
        }

        [HttpDelete]

        public string DeleteTagTask( int id)
        {
            return $"deleted tag task with {id} Id";
        }
    }
}
