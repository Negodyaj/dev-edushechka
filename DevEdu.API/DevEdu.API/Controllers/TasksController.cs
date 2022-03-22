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

        public TasksController(
            IMapper mapper,
            ITaskService taskService)
        {
            _taskService = taskService;
            _mapper = mapper;
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
            var homeworkDto = _mapper.Map<HomeworkDto>(model.Homework);
            var task = await _taskService.AddTaskByTeacherAsync(taskDto, homeworkDto, model.GroupId, userIdentityInfo);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return Created(new Uri($"api/Task/{output.Id}", UriKind.Relative), output);
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
            return Created(new Uri($"api/Task/{output.Id}", UriKind.Relative), output);
        }

        // api/tasks/{taskId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("teacher/{taskId}")]
        [Description("Update task for Teacher")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TaskInfoOutputModel> UpdateTaskByTeacherAsync(int taskId, [FromBody] TaskInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            var taskUpdate = await _taskService.UpdateTaskAsync(taskDto, taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoOutputModel>(taskUpdate);
        }

        // api/tasks/{taskId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPut("methodist/{taskId}")]
        [Description("Update task for Methodist")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TaskInfoOutputModel> UpdateTaskByMethodistAsync(int taskId, [FromBody] TaskInputModel model)
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
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
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

        //  api/tasks/1/with-courses 
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("{taskId}/with-groups")]
        [Description("Get task by Id with groups")]
        [ProducesResponseType(typeof(TaskInfoWithGroupsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithGroupsOutputModel> GetTaskWithGroupsAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithGroupsByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithGroupsOutputModel>(taskDto);
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
    }
}