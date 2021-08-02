using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Extensions;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
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
        public HomeworkInfoOutputModel AddHomework(int groupId, int taskId, [FromBody] HomeworkInputModel model)
        {
            var userId = this.GetUserId();
            var dto = _mapper.Map<HomeworkDto>(model);
            var hw = _homeworkService.AddHomework(groupId, taskId, dto, userId);
            return _mapper.Map<HomeworkInfoOutputModel>(hw);
        }

        //  api/homework/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete homework")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteHomework(int id)
        {
            var userId = this.GetUserId();
            _homeworkService.DeleteHomework(id, userId);
        }

        //  api/homework/1
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{id}")]
        [Description("Update homework")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public HomeworkInfoOutputModel UpdateHomework(int id, [FromBody] HomeworkInputModel model)
        {
            var userId = this.GetUserId();
            var dto = _mapper.Map<HomeworkDto>(model);
            var output = _homeworkService.UpdateHomework(id, dto, userId);
            return _mapper.Map<HomeworkInfoOutputModel>(output);
        }
    }
}