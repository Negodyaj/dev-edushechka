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

        // api/task
        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {

            return 1;
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public string DeleteTask(int taskId)
        {
            return $"deleted task with {taskId} Id";
        }

        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public string UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {

            return $"update task with {taskId} Id";
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpPost("{taskId}/tag/{tagId}")]
        public int AddTagToTask(int taskId, int tagId)
        {
            return 1;
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        public string DeleteTagFromTask(int taskId, int tagId)
        {
            return $"deleted tag task with {taskId} taskId";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPost("{taskId}/student/{studentId}")]
        public string AddStudentAnswerOnTask(int taskId, int studentId, string taskAnswer)  // to inputModel
        {
            return $"add answer for task {taskId} id";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]  // to inputModel
        public string UpdateStudentAnswerOnTask(int studentId, int taskId, string taskAnswer)
        {
            return $"update task with {taskId} id by {taskAnswer}";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            return $"deleted answer for task {taskId} id";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            return statusId;
        }

        // api/task/{taskId}/student/{studentId}/comment}
        [HttpPost("{taskId}/student/{studentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskId, int studentId, [FromBody] CommentAddtInputModel inputModel)
        {
            return taskId;
        }
    }
}
