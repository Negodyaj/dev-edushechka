using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        
        public TaskController(IMapper mapper, ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        //  api/Task/1
        [HttpGet("{taskId}")]
        public TaskDto GetTask(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            return task;
        }

        //  api/Task
        [HttpGet]
        public List<TaskDto> GetAllTasks()
        {
            var taskDtos = _taskRepository.GetTasks();
            return taskDtos;
        }

        // api/task
        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            return _taskRepository.AddTask(taskDto);
        }


        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public void UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public void DeleteTask(int taskId)
        {
            _taskRepository.DeleteTask(taskId);
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
        public int AddCommentOnStudentAnswer(int taskId, int studentId, [FromBody] CommentAddInputModel inputModel)
        {
            return taskId;
        }
    }
}