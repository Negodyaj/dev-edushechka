using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        //  api/homework/1
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [HttpGet("{id}")]
        [Description("Return homework by id")]
        [ProducesResponseType(typeof(HomeworkInfoFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public HomeworkInfoFullOutputModel GetHomework(int id)
        {
            var userId = this.GetUserId();
            var dto = _homeworkService.GetHomework(id, userId);
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
        public List<HomeworkInfoWithTaskOutputModel> GetHomeworkByGroupId(int groupId)
        {
            var userId = this.GetUserId();
            var dto = _homeworkService.GetHomeworkByGroupId(groupId, userId);
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
        public List<HomeworkInfoWithGroupOutputModel> GetHomeworkByTaskId(int taskId)
        {
            var dto = _homeworkService.GetHomeworkByTaskId(taskId);
            var output = _mapper.Map<List<HomeworkInfoWithGroupOutputModel>>(dto);
            return output;
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
            var userId = this.GetUserId();
            var dto = _mapper.Map<HomeworkDto>(model);
            var hw = _homeworkService.AddHomework(groupId, taskId, dto, userId);
            var output = _mapper.Map<HomeworkInfoOutputModel>(hw);
            return StatusCode(201, output);
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
            var userId = this.GetUserId();
            _homeworkService.DeleteHomework(id, userId);
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
            var userId = this.GetUserId();
            var dto = _mapper.Map<HomeworkDto>(model);
            var output = _homeworkService.UpdateHomework(id, dto, userId);
            return _mapper.Map<HomeworkInfoOutputModel>(output);
        }
    }
}