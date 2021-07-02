using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private TaskRepository _taskRepository;
        private MyMapper _mapper;
        public TaskController()
        {
            _taskRepository = new TaskRepository();
            _mapper = new MyMapper();
        }

        //  api/Task/1
        [HttpGet("{taskId}")]
        public TaskDto GetTask(int taskId)
        {
            TaskDto task = _taskRepository.GetTaskById(taskId);
            return task;
        }

        //  api/Task
        [HttpGet]
        public List<TaskDto> GetAllTasks()
        {
            return _taskRepository.GetTasks();
        }

        // api/task
        [HttpPost]
        public string AddTask([FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.SingleMapping<TaskInputModel,TaskDto>(model);
            _taskRepository.AddTask(taskDto);
            return $"Добавлено задание {taskDto.Name} {taskDto.Description} {taskDto.StartDate} {taskDto.EndDate} {taskDto.Links} {taskDto.IsRequired}";
        }


        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public string UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.SingleMapping<TaskInputModel, TaskDto>(model);
            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
            return $"Обновлено задание с Id: {taskDto.Id} {taskDto.Name} {taskDto.Description} {taskDto.StartDate} {taskDto.EndDate} {taskDto.Links} {taskDto.IsRequired}";
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public string DeleteTask(int taskId)
        {
            _taskRepository.DeleteTask(taskId);
            return $"Удалено задание {taskId}";
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
