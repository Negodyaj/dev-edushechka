using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IHomeworkService _homeworkService;

        public TasksController(
            IMapper mapper,
            ITaskService taskService,
            IHomeworkService homeworkService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _homeworkService = homeworkService;
        }

        // api/tasks/teacher
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("teacher")]
        [Description("Add new task by teacher")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TaskInfoOutputModel>> AddTaskByTeacherAsync([FromBody] TaskByTeacherInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            var task = await _taskService.AddTaskByTeacherAsync(taskDto, model.GroupId, userIdentityInfo);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return Created(new Uri($"api/Tasks/{output.Id}", UriKind.Relative), output);
        }

        // api/tasks/teacher
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("publish-homework")]
        [Description("Add new task by teacher and publish a homework")]
        [ProducesResponseType(typeof(HomeworkInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<HomeworkInfoOutputModel>> AddTaskByTeacherAndPublishAsync([FromBody] TaskByTeacherWithDatesRequest model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            var task = await _taskService.AddTaskByTeacherAsync(taskDto, model.GroupId, userIdentityInfo);
            var hw = new HomeworkInputModel { StartDate = model.StartDate, EndDate = model.EndDate };
            var hwDto = await _homeworkService.AddHomeworkAsync(model.GroupId, task.Id, _mapper.Map<HomeworkDto>(hw), userIdentityInfo);
            var output = _mapper.Map<HomeworkInfoOutputModel>(hwDto);
            return Created(new Uri($"api/Homeworks/{output.Id}", UriKind.Relative), output);
        }

        // api/tasks/methodist
        [AuthorizeRoles(Role.Methodist)]
        [HttpPost("methodist")]
        [Description("Add new task by methodist")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<TaskInfoOutputModel>> AddTaskByMethodistAsync([FromBody] TaskByMethodistInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            var task = await _taskService.AddTaskByMethodistAsync(taskDto, model.CourseIds, userIdentityInfo);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return Created(new Uri($"api/Tasks/{output.Id}", UriKind.Relative), output);
        }

        // api/tasks/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPut("{taskId}")]
        [Description("Update an existing task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TaskInfoOutputModel> UpdateTaskAsync(int taskId, [FromBody] TaskInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            var taskUpdate = await _taskService.UpdateTaskAsync(taskDto, taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoOutputModel>(taskUpdate);
        }

        // api/tasks/{taskId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{taskId}")]
        [Description("Delete task with selected Id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTaskAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            await _taskService.DeleteTaskAsync(taskId, userIdentityInfo);
            return NoContent();
        }

        //  api/tasks/{Id}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpGet("{id}")]
        [Description("Get task by Id")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoOutputModel> GetTaskByIdAsync(int id)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskByIdAsync(id, userIdentityInfo);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/tasks/by-course/{courseId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("by-course/{courseId}")]
        [Description("Get task by CourseId (for methodist)")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<TaskInfoOutputModel>> GetTasksByCourseIdAsync(int courseId)
        {
            var taskDto = await _taskService.GetTasksByCourseIdAsync(courseId);
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDto);
        }

        //  api/tasks/by-group/{groupId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("by-group/{groupId}")]
        [Description("Get task by GroupId (for teacher)")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<TaskInfoOutputModel>> GetTasksByGroupIdAsync(int groupId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTasksByGroupIdAsync(groupId, userIdentityInfo);
            return _mapper.Map<List<TaskInfoOutputModel>>(taskDto);
        }

        //  api/tasks/1/with-courses 
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with courses")]
        [ProducesResponseType(typeof(TaskInfoWithCoursesOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithCoursesOutputModel> GetTaskWithCoursesAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/tasks/1/with-answers 
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with answers")]
        [ProducesResponseType(typeof(TaskInfoWithAnswersOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithAnswersOutputModel> GetTaskWithAnswersAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithAnswersByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/tasks 
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<TaskInfoOutputModel>> GetAllTasksAsync()
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var list = await _taskService.GetTasksAsync(userIdentityInfo);
            return _mapper.Map<List<TaskInfoOutputModel>>(list);
        }

        //  api/tasks/1/answer 
        [AuthorizeRoles(Role.Student)]
        [HttpGet("{taskId}/answer")]
        [Description("Get student answer on task (for student)")]
        [ProducesResponseType(typeof(StudentHomeworkWithTaskOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<StudentHomeworkWithTaskOutputModel> GetStudentAnswerOnTaskAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var answer = await _taskService.GetStudentAnswerOnTaskAsync(taskId, userIdentityInfo);
            return _mapper.Map<StudentHomeworkWithTaskOutputModel>(answer);
        }
    }
}