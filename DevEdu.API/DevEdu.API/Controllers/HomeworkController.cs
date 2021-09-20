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

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHomeworkService _homeworkService;

        public HomeworkController(IMapper mapper, IHomeworkService homeworkService)
        {
            _mapper = mapper;
            _homeworkService = homeworkService;
        }

        //  api/homework/{id}
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Return homework by id")]
        [ProducesResponseType(typeof(HomeworkInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public HomeworkInfoFullOutputModel GetHomework(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _homeworkService.GetHomeworkAsync(id, userInfo);
            var output = _mapper.Map<HomeworkInfoFullOutputModel>(dto);
            return output;
        }

        //  api/homework/by-group/{groupId}
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("by-group/{groupId}")]
        [Description("Get all homework by group")]
        [ProducesResponseType(typeof(List<HomeworkInfoWithTaskOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<HomeworkInfoWithTaskOutputModel> GetHomeworksByGroupId(int groupId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _homeworkService.GetHomeworkByGroupIdAsync(groupId, userInfo);
            var output = _mapper.Map<List<HomeworkInfoWithTaskOutputModel>>(dto);
            return output;
        }

        //  api/homework/by-task/{taskId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpGet("by-task/{taskId}")]
        [Description("Get all homework by task")]
        [ProducesResponseType(typeof(List<HomeworkInfoWithGroupOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<HomeworkInfoWithGroupOutputModel> GetHomeworksByTaskId(int taskId)
        {
            var list = _homeworkService.GetHomeworkByTaskIdAsync(taskId);
            return _mapper.Map<List<HomeworkInfoWithGroupOutputModel>>(list);
        }

        //  api/homework/group/1/task/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("group/{groupId}/task/{taskId}")]
        [Description("Add homework")]
        [ProducesResponseType(typeof(HomeworkInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<HomeworkInfoOutputModel> AddHomework(int groupId, int taskId, [FromBody] HomeworkInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<HomeworkDto>(model);
            var hw = _homeworkService.AddHomeworkAsync(groupId, taskId, dto, userInfo);
            var output = _mapper.Map<HomeworkInfoOutputModel>(hw);
            return Created(new Uri($"api/Homework/{output.Id}", UriKind.Relative), output);
        }

        //  api/homework/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete homework")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public ActionResult DeleteHomework(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            _homeworkService.DeleteHomeworkAsync(id, userInfo);
            return NoContent();
        }

        //  api/homework/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{id}")]
        [Description("Update homework")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public HomeworkInfoOutputModel UpdateHomework(int id, [FromBody] HomeworkInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<HomeworkDto>(model);
            var output = _homeworkService.UpdateHomeworkAsync(id, dto, userInfo);
            return _mapper.Map<HomeworkInfoOutputModel>(output);
        }
    }
}