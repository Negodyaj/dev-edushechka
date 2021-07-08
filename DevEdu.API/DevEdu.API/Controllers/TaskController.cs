using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskRepository _taskRepository;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        
        public TaskController(IMapper mapper, ITaskRepository taskRepository, IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
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
            return _taskRepository.AddTagToTagTask(taskId, tagId);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        public string DeleteTagFromTask(int taskId, int tagId)
        {
            _taskRepository.DeleteTagFromTask(taskId, tagId);
            return $"deleted tag task with {taskId} taskId";
        }

        // api/task/{taskId}/student/{studentId}
        [HttpPost("{taskId}/student/{studentId}")]
        public void AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);

        }

        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(taskAnswerDto);

            return taskId;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(taskId, studentId);

            return $"Deleted answer for task {taskId} id.";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId)
        {
            _studentAnswerOnTaskRepository.UpdateStatusAnswerOnTask(taskId, studentId, statusId);

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