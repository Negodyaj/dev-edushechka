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

        [HttpPost("{taskId}/tag/{tagId}")]

        public int AddTagTask(int taskId, int tagId)
        {
            return 1;
        }

        [HttpDelete("{id}")]
        public string DeleteTagTask( int id)
        {
            return $"deleted tag task with {id} Id";
        }


        [HttpPost("student/{studentId}/task/{taskId}/task-answer/{taskAnswer}")]
        public string AddAnswerTaskStudent(int studentId, int taskId, string taskAnswer)
        {
            return $"add answer for task {taskId} id";
        }

        [HttpPut("student/{studentId}/task/{taskId}/task-answer/{taskAnswer}")]
        public string UpdateAnswerTaskStudent(int studentId, int taskId, string taskAnswer)
        {
            return $"update task with {taskId} id by {taskAnswer}";
        }

        [HttpDelete("student/{studentId}/task/{taskId}")]
        public string DeleteAnswerTaskStudent(int studentId, int taskId)
        {
            return $"deleted answer for task {taskId} id";
        }

        [HttpPost("student/{studentId}/task/{taskId}/task-answer/status-task/{statusId}")]
        public int AddStatusTaskStudent(int studentId, int taskId, int statusId)
        {
            return statusId;
        }

        [HttpPut("student/{studentId}/task/{taskId}/task-answer/status-task/{statusId}")]
        public int UpdateStatusTaskStudent(int studentId, int taskId, int statusId)
        {
            return statusId;
        }
    }
}
