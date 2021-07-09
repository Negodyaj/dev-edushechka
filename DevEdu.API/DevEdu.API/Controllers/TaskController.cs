using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;

        public TaskController(
            IMapper mapper, 
            ITaskService taskService, 
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository, 
            ITaskRepository taskRepository,
            ICommentRepository commentRepository)
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _taskRepository = taskRepository;
            _commentRepository = commentRepository;
        }

        //  api/Task/1
        [HttpGet("{taskId}")]
        public TaskDto GetTask(int taskId)
        {
            var taskDto = _taskService.GetTaskById(taskId);
            return taskDto;
        }

        //  api/Task
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoOutputModel))]
        public List<TaskInfoOutputModel> GetAllTasks()
        {
            var taskDtos = _taskService.GetTasks();
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task
        [HttpPost]
        public int AddTask([FromBody] TaskInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            return _taskService.AddTask(taskDto);
        }


        // api/task/{taskId}
        [HttpPut("{taskId}")]
        public void UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            _taskService.UpdateTask(taskId, taskDto);
        }

        // api/task/{taskId}
        [HttpDelete("{taskId}")]
        public void DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
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
        public void AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskOutputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.TaskId = taskId;
            taskAnswerDto.StudentId = studentId;

            _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);

        }

        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskOutputModel inputModel)
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

        // api/task/answer/{taskStudentId}/comment}
        [HttpPost("answer/{taskStudentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskstudentId, [FromBody] CommentAddInputModel inputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(inputModel);
            int commentId = _commentRepository.AddComment(commentDto);
            _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskstudentId, commentId);

            return taskstudentId;
        }
    }
}