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

        // api/StudentHomework/task/{taskId}/student/{studentId} 
        [HttpPost("{taskId}/student/{studentId}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Add student homework")]
        [ProducesResponseType(typeof(StudentHomeworkFullOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<StudentHomeworkFullOutputModel> AddStudentAnswerOnTask(int taskId, int studentId, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var taskAnswerDto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var studentHomeworkId = _studentHomeworkService.AddStudentAnswerOnTask(taskId, studentId, taskAnswerDto, userInfo);
            var studentAnswerDto = _studentHomeworkService.GetStudentHomeworkId(studentHomeworkId, userInfo);
            var output = _mapper.Map<StudentHomeworkFullOutputModel>(studentAnswerDto);

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
        public ActionResult DeleteStudentAnswerOnTask(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            _studentHomeworkService.DeleteStudentAnswerOnTask(id, userInfo);

            return NoContent();
        }

        // api/StudentHomework/{id} 
        [HttpPut("{id}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Update student answer")]
        [ProducesResponseType(typeof(StudentHomeworkFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public StudentHomeworkFullOutputModel UpdateStudentAnswerOnTask(int id, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var taskAnswerDto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var output = _studentHomeworkService.UpdateStudentAnswerOnTask(id, taskAnswerDto, userInfo);

            return _mapper.Map<StudentHomeworkFullOutputModel>(output);
        }

        // api/StudentHomework/{id}/change-status/{statusId} 
        [HttpPut("{id}/change-status/{statusId}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [Description("Update homework status")]
        [ProducesResponseType(typeof(StudentHomeworkFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public StudentHomeworkFullOutputModel UpdateStatusOfStudentAnswer(int id, int statusId)
        {
            var userInfo = this.GetUserIdAndRoles();
            _studentHomeworkService.ChangeStatusOfStudentAnswerOnTask(id, statusId, userInfo);
            var output = _studentHomeworkService.GetStudentHomeworkId(id, userInfo);

            return _mapper.Map<StudentHomeworkFullOutputModel>(output);
        }

        // api/StudentHomework/{id}/1
        [HttpGet("{id}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor)]
        [Description("Get student homework by id")]
        [ProducesResponseType(typeof(StudentHomeworkFullOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public StudentHomeworkFullOutputModel GetStudentAnswerOnTaskByTaskIdAndStudentId(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var studentAnswerDto = _studentHomeworkService.GetStudentHomeworkId(id, userInfo);
            var output = _mapper.Map<StudentHomeworkFullOutputModel>(studentAnswerDto);

            return output;
        }

        // api/StudentHomework/{taskId}/all-answers 
        [HttpGet("{taskId}/all-answers")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist)]
        [Description("Get all student homework on task by task")]
        [ProducesResponseType(typeof(List<StudentHomeworkFullOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentHomeworkFullOutputModel> GetAllStudentAnswersOnTask(int taskId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var studentAnswersDto = _studentHomeworkService.GetAllStudentAnswersOnTask(taskId, userInfo);
            var output = _mapper.Map<List<StudentHomeworkFullOutputModel>>(studentAnswersDto);

            return output;
        }

        // api/StudentHomework/answer/by-user/42 
        [HttpGet("by-user/{userId}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student, Role.Methodist)]
        [Description("Get all answers of student")]
        [ProducesResponseType(typeof(List<StudentHomeworkOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<StudentHomeworkOutputModel> GetAllAnswersByStudentId(int userId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var answersDto = _studentHomeworkService.GetAllAnswersByStudentId(userId, userInfo);
            var output = _mapper.Map<List<StudentHomeworkOutputModel>>(answersDto);

            return output;
        }
    }
}