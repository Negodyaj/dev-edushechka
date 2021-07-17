using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IStudentAnswerOnTaskService _studentAnswerOnTaskService;
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;

        public TaskController(
            IMapper mapper, 
            ITaskService taskService,
            IStudentAnswerOnTaskService studentAnswerOnTaskService, 
            ITaskRepository taskRepository,
            ICommentRepository commentRepository)
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentAnswerOnTaskService = studentAnswerOnTaskService;
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
        public List<TaskDto> GetAllTasks()
        {
            var taskDtos = _taskService.GetTasks();
            return taskDtos;
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
        public void AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.Task.Id = taskId;
            taskAnswerDto.User.Id = studentId;

            _studentAnswerOnTaskService.AddStudentAnswerOnTask(taskAnswerDto);

        }

        //  api/task
        [HttpGet]
        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask()
        {
            var studentStatusDto = _studentAnswerOnTaskService.GetAllStudentAnswersOnTask();
            return studentStatusDto;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpGet("{taskId}/student/{studentId}")]
        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId, StudentAnswerOnTaskDto dto)
        {
            dto.Task.Id = taskId;
            dto.User.Id = studentId;

            var studentStatusDto = _studentAnswerOnTaskService.GetStudentAnswerOnTaskByTaskIdAndStudentId(dto);
            return studentStatusDto;
        }


        // api/task/{taskId}/student/{studentId}
        [HttpPut("{taskId}/student/{studentId}")]
        public int UpdateStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentAnswerOnTaskInputModel inputModel)
        {
            var taskAnswerDto = _mapper.Map<StudentAnswerOnTaskDto>(inputModel);
            taskAnswerDto.Task.Id = taskId;
            taskAnswerDto.User.Id = studentId;

            _studentAnswerOnTaskService.UpdateStudentAnswerOnTask(taskAnswerDto);

            return taskId;
        }

        // api/task/{taskId}/student/{studentId}
        [HttpDelete("{taskId}/student/{studentId}")]
        public string DeleteStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto dto)
        {
            dto.Task.Id = taskId;
            dto.User.Id = studentId;

            _studentAnswerOnTaskService.DeleteStudentAnswerOnTask(dto);

            return $"Deleted answer for task {taskId} id.";
        }

        // api/task/{taskId}/student/{studentId}/change-status/{statusId}
        [HttpPut("{taskId}/student/{studentId}/change-status/{statusId}")]
        public int UpdateStatusOfStudentAnswer(int taskId, int studentId, int statusId, StudentAnswerOnTaskDto dto)
        {
            dto.Task.Id = taskId;
            dto.User.Id = studentId;

            _studentAnswerOnTaskService.ChangeStatusOfStudentAnswerOnTask(dto, statusId);

            return statusId;
        }

        // api/task/answer/{taskStudentId}/comment}
        [HttpPost("answer/{taskStudentId}/comment")]
        public int AddCommentOnStudentAnswer(int taskstudentId, [FromBody] CommentAddInputModel inputModel)
        {
            var commentDto = _mapper.Map<CommentDto>(inputModel);
            int commentId = _commentRepository.AddComment(commentDto);
            _studentAnswerOnTaskService.AddCommentOnStudentAnswer(taskstudentId, commentId);

            return taskstudentId;
        }
    }
}