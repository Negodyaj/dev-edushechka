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
    public class TaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;

        public TaskController(
            IMapper mapper,
            ITaskService taskService)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        // api/task/teacher
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
            var task = await _taskService.AddTaskByTeacherAsync(taskDto, homeworkDto, model.GroupId, model.Tags, userIdentityInfo);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return Created(new Uri($"api/Task/{output.Id}", UriKind.Relative), output);
        }

        // api/task/methodist
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
            var task = await _taskService.AddTaskByMethodistAsync(taskDto, model.CourseIds, model.Tags, userIdentityInfo);
            var output = _mapper.Map<TaskInfoOutputModel>(task);
            return Created(new Uri($"api/Task/{output.Id}", UriKind.Relative), output);
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("teacher/{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TaskInfoOutputModel> UpdateTaskByTeacherAsync(int taskId, [FromBody] TaskInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(await _taskService.UpdateTaskAsync(taskDto, taskId, userIdentityInfo));
        }

        // api/task/{taskId}
        [AuthorizeRoles(Role.Methodist)]
        [HttpPut("methodist/{taskId}")]
        [Description("Update task")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<TaskInfoOutputModel> UpdateTaskByMethodistAsync(int taskId, [FromBody] TaskInputModel model)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = _mapper.Map<TaskDto>(model);
            return _mapper.Map<TaskInfoOutputModel>(await _taskService.UpdateTaskAsync(taskDto, taskId, userIdentityInfo));
        }

        // api/task/{taskId}
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

        //  api/task/{Id}
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Get task by Id with tags")]
        [ProducesResponseType(typeof(TaskInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoOutputModel> GetTaskWithTagsAsync(int id)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskByIdAsync(id, userIdentityInfo);
            return _mapper.Map<TaskInfoOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses 
        [AuthorizeRoles(Role.Methodist)]
        [HttpGet("{taskId}/with-courses")]
        [Description("Get task by Id with tags and courses")]
        [ProducesResponseType(typeof(TaskInfoWithCoursesOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithCoursesOutputModel> GetTaskWithTagsAndCoursesAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithCoursesByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithCoursesOutputModel>(taskDto);
        }

        //  api/Task/1/with-answers 
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [HttpGet("{taskId}/with-answers")]
        [Description("Get task by Id with tags and answers")]
        [ProducesResponseType(typeof(TaskInfoWithAnswersOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithAnswersOutputModel> GetTaskWithTagsAndAnswersAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithAnswersByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithAnswersOutputModel>(taskDto);
        }

        //  api/Task/1/with-courses 
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("{taskId}/with-groups")]
        [Description("Get task by Id with tags and groups")]
        [ProducesResponseType(typeof(TaskInfoWithGroupsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<TaskInfoWithGroupsOutputModel> GetTaskWithTagsAndGroupsAsync(int taskId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var taskDto = await _taskService.GetTaskWithGroupsByIdAsync(taskId, userIdentityInfo);
            return _mapper.Map<TaskInfoWithGroupsOutputModel>(taskDto);
        }

        //  api/Task 
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet]
        [Description("Get all tasks with tags")]
        [ProducesResponseType(typeof(List<TaskInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public async Task<List<TaskInfoOutputModel>> GetAllTasksWithTagsAsync()
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            var list = await _taskService.GetTasksAsync(userIdentityInfo);
            return _mapper.Map<List<TaskInfoOutputModel>>(list);
        }

        // api/task/{taskId}/tag/{tagId} 
        [HttpPost("{taskId}/tag/{tagId}")]
        [Description("Add tag to task")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddTagToTaskAsync(int taskId, int tagId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            await _taskService.AddTagToTaskAsync(taskId, tagId, userIdentityInfo);
            return NoContent();
        }

        // api/task/{taskId}/tag/{tagId} 
        [HttpDelete("{taskId}/tag/{tagId}")]
        [Description("Delete tag from task")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTagFromTaskAsync(int taskId, int tagId)
        {
            var userIdentityInfo = this.GetUserIdAndRoles();
            await _taskService.DeleteTagFromTaskAsync(taskId, tagId, userIdentityInfo);
            return NoContent();
        }
    }
}