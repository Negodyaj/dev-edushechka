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
    public class StudentHomeworkController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStudentHomeworkService _studentHomeworkService;

        public StudentHomeworkController(
            IMapper mapper,
            IStudentHomeworkService studentHomeworkService)
        {
            _mapper = mapper;
            _studentHomeworkService = studentHomeworkService;
        }

        // api/StudentHomework/task/{taskId} 
        [HttpPost("/homework/{homeworkId}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Add student homework")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<StudentHomeworkWithHomeworkOutputModel> AddStudentHomework(int homeworkId, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var returnedDto = _studentHomeworkService.AddStudentHomework(homeworkId, dto, userInfo);
            var output = _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(returnedDto);
            return StatusCode(201, output);
        }

        // api/StudentHomework/{id} 
        [HttpDelete("{id}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Delete student homework")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult DeleteStudentHomework(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            _studentHomeworkService.DeleteStudentHomework(id, userInfo);
            return NoContent();
        }

        // api/StudentHomework/{id} 
        [HttpPut("{id}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Update student answer")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public StudentHomeworkWithHomeworkOutputModel UpdateStudentHomework(int id, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var taskAnswerDto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var output = _studentHomeworkService.UpdateStudentHomework(id, taskAnswerDto, userInfo);
            return _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(output);
        }

        // api/StudentHomework/{id}/change-status/{statusId} 
        [HttpPut("{id}/change-status/{statusId}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Update homework status")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public StudentHomeworkWithHomeworkOutputModel UpdateStatusOfStudentHomework(int id, int statusId)
        {
            var userInfo = this.GetUserIdAndRoles();
            _studentHomeworkService.UpdateStatusOfStudentHomework(id, statusId, userInfo);
            var output = _studentHomeworkService.GetStudentHomeworkById(id, userInfo);
            return _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(output);
        }

        // api/StudentHomework/{id}/1
        [HttpGet("{id}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Get student homework by id")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public StudentHomeworkWithHomeworkOutputModel GetStudentHomeworkById(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var studentAnswerDto = _studentHomeworkService.GetStudentHomeworkById(id, userInfo);
            var output = _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(studentAnswerDto);
            return output;
        }

        // api/StudentHomework/task/{taskId}/answers 
        [HttpGet("task/{taskId}/answers")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist)]
        [Description("Get all students homework on task by task")]
        [ProducesResponseType(typeof(List<StudentHomeworkOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentHomeworkOutputModel> GetAllStudentHomeworkOnTask(int taskId)
        {
            var studentAnswersDto = _studentHomeworkService.GetAllStudentHomeworkOnTask(taskId);
            var output = _mapper.Map<List<StudentHomeworkOutputModel>>(studentAnswersDto);
            return output;
        }

        // api/StudentHomework/answer/by-user/42 
        [HttpGet("by-user/{userId}")]
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Get all answers of student")]
        [ProducesResponseType(typeof(List<StudentHomeworkWithTaskOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentHomeworkWithTaskOutputModel> GetAllStudentHomeworkByStudentId(int userId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var answersDto = _studentHomeworkService.GetAllStudentHomeworkByStudentId(userId, userInfo);
            var output = _mapper.Map<List<StudentHomeworkWithTaskOutputModel>>(answersDto);
            return output;
        }
    }
}