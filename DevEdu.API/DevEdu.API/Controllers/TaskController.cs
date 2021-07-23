using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.API.Models.InputModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using AutoMapper.Internal;
using Dapper;
using DevEdu.API.Common;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Repositories;
using DevEdu.DAL.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DevEdu.API.Controllers
{
    [AuthorizeRoles()]
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
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{taskId}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel GetTaskWithTags(int taskId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var taskDto = _taskService.GetTaskById(taskId);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/courses
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithCoursesOutputModel))]
        public TaskInfoWithCoursesOutputModel GetTaskWithTagsAndCourses(int taskId)
        {
            var taskDto = _taskService.GetTaskWithCoursesById(taskId);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/answers
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoWithAnswersOutputModel))]
        public TaskInfoWithAnswersOutputModel GetTaskWithTagsAndAnswers(int taskId)
        {
            var taskDto = _taskService.GetTaskWithAnswersById(taskId);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskInfoOutputModel))]
        public List<TaskInfoOutputModel> GetAllTasksWithTags()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            //var userRoles = User.Claims(c => c.Role == ClaimTypes.Role)?.;
            //var userRoles = claimsIdentity.FindAll(ClaimTypes.Role)?.AsList();
            var taskDtos = _taskService.GetTasks();
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPost]
        [Description("Add new task")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public TaskInfoOutputModel AddTask([FromBody] TaskInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.GetTaskById(_taskService.AddTask(taskDto)));
        }


        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPut("{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public TaskInfoOutputModel UpdateTask(int taskId, [FromBody] TaskInputModel model)
        {
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            taskDto.Id = taskId;
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpPost("{taskId}/tag/{tagId}")]
        [Description("Add tag to task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddTagToTask(int taskId, int tagId)
        {
            _taskService.AddTagToTask(taskId, tagId);
        }

        // api/task/{taskId}/tag/{tagId}
        [HttpDelete("{taskId}/tag/{tagId}")]
        [Description("Delete tag from task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTagFromTask(int taskId, int tagId)
        {
            _taskService.DeleteTagFromTask(taskId, tagId);
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