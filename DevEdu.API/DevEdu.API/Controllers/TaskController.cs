using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Extensions;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IStudentHomeworkService _studentHomeworkService;
        private readonly ICommentService _commentService;

        public TaskController(
            IMapper mapper,
            ITaskService taskService,
            IStudentHomeworkService studentHomeworkService,
            ICommentService commentService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _studentHomeworkService = studentHomeworkService;
            _commentService = commentService;
        }

        // api/task/teacher
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("teacher")]
        [Description("Add new task by teacher")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<TaskInfoOutputModel> AddTaskByTeacher([FromBody] TaskByTeacherInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            var homeworkDto = _mapper.Map<HomeworkDto>(model.Homework);
            var task = _taskService.AddTaskByTeacher(taskDto, homeworkDto, model.GroupId, model.Tags);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return StatusCode(201, output);
        }

        // api/task/methodist
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("methodist")]
        [Description("Add new task by methodist")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<TaskInfoOutputModel> AddTaskByMethodist([FromBody] TaskByMethodistInputModel model)
        {
            var taskDto = _mapper.Map<TaskDto>(model);
            var task = _taskService.AddTaskByMethodist(taskDto, model.CourseIds, model.Tags);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return StatusCode(201, output);
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("teacher/{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoOutputModel UpdateTaskByTeacher(int taskId, [FromBody] TaskByTeacherUpdateInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto, taskId, userIdentityInfo));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPut("methodist/{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TaskInfoOutputModel UpdateTaskByMethodist(int taskId, [FromBody] TaskInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            TaskDto taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(_taskService.UpdateTask(taskDto, taskId, userIdentityInfo));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteTask(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            _taskService.DeleteTask(taskId, userIdentityInfo);
            return NoContent();
        }


        //  api/Task/1 
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{taskId}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public TaskInfoOutputModel GetTaskWithTags(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _taskService.GetTaskById(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses 
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(typeof(TaskInfoWithCoursesOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public TaskInfoWithCoursesOutputModel GetTaskWithTagsAndCourses(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _taskService.GetTaskWithCoursesById(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/1/with-answers 
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(typeof(TaskInfoWithAnswersOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public TaskInfoWithAnswersOutputModel GetTaskWithTagsAndAnswers(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _taskService.GetTaskWithAnswersById(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses 
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("{taskId}/with-groups")]
        [Description("Get task by Id with tags and groups")]
        [ProducesResponseType(typeof(TaskInfoWithGroupsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public TaskInfoWithGroupsOutputModel GetTaskWithTagsAndGroups(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _taskService.GetTaskWithGroupsById(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithGroupsOutputModel>(taskDto);
        }

        //  api/Task 
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<TaskInfoOutputModel> GetAllTasksWithTags()
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDtos = _taskService.GetTasks(userIdentityInfo);
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDtos);
        }

        // api/task/{taskId}/tag/{tagId} 
        [HttpPost("{taskId}/tag/{tagId}")]
        [Description("Add tag to task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void AddTagToTask(int taskId, int tagId)
        {
            _taskService.AddTagToTask(taskId, tagId);
        }

        // api/task/{taskId}/tag/{tagId} 
        [HttpDelete("{taskId}/tag/{tagId}")]
        [Description("Delete tag from task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteTagFromTask(int taskId, int tagId)
        {
            _taskService.DeleteTagFromTask(taskId, tagId);
        }
    }
}