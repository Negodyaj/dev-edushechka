using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration.ExceptionResponses;
using DevEdu.API.Extensions;
using DevEdu.API.Models;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

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
        [HttpPost("{homeworkId}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Add student homework")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<StudentHomeworkWithHomeworkOutputModel>> AddStudentHomeworkAsync(int homeworkId, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var returnedDto = await _studentHomeworkService.AddStudentHomeworkAsync(homeworkId, dto, userInfo);
            var output = _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(returnedDto);
            return Created(new Uri($"api/StudentHomework/by-user/{output.Id}", UriKind.Relative), output);
        }

        // api/StudentHomework/{id} 
        [HttpDelete("{id}")]
        [AuthorizeRoles(Role.Student)]
        [Description("Delete student homework")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult> DeleteStudentHomeworkAsync(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            await _studentHomeworkService.DeleteStudentHomeworkAsync(id, userInfo);
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
        public async Task<StudentHomeworkWithHomeworkOutputModel> UpdateStudentHomeworkAsync(int id, [FromBody] StudentHomeworkInputModel inputModel)
        {
            var userInfo = this.GetUserIdAndRoles();
            var taskAnswerDto = _mapper.Map<StudentHomeworkDto>(inputModel);
            var studentHomeworkDto = await _studentHomeworkService.UpdateStudentHomeworkAsync(id, taskAnswerDto, userInfo);
            return _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(studentHomeworkDto);
        }

        // api/StudentHomework/{id}/change-status/{statusId} 
        [HttpPut("{id}/change-status/{statusId}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Update homework status")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<StudentHomeworkWithHomeworkOutputModel> UpdateStatusOfStudentHomeworkAsync(int id, int statusId)
        {
            var userInfo = this.GetUserIdAndRoles();
            await _studentHomeworkService.UpdateStatusOfStudentHomeworkAsync(id, statusId, userInfo);
            var dto = await _studentHomeworkService.GetStudentHomeworkByIdAsync(id, userInfo);
            return _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(dto);
        }

        // api/StudentHomework/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Get student homework by id")]
        [ProducesResponseType(typeof(StudentHomeworkWithHomeworkOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<StudentHomeworkWithHomeworkOutputModel> GetStudentHomeworkByIdAsync(int id)
        {
            var userInfo = this.GetUserIdAndRoles();
            var studentAnswerDto = await _studentHomeworkService.GetStudentHomeworkByIdAsync(id, userInfo);
            return _mapper.Map<StudentHomeworkWithHomeworkOutputModel>(studentAnswerDto);
        }

        // api/StudentHomework/task/{taskId}/answers 
        [HttpGet("task/{taskId}/answers")]
        [AuthorizeRoles(Role.Teacher, Role.Tutor, Role.Methodist)]
        [Description("Get all students homework on task by task")]
        [ProducesResponseType(typeof(List<StudentHomeworkOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<StudentHomeworkOutputModel>> GetAllStudentHomeworkOnTaskAsync(int taskId)
        {
            var studentAnswersDto = await _studentHomeworkService.GetAllStudentHomeworkOnTaskAsync(taskId);
            return _mapper.Map<List<StudentHomeworkOutputModel>>(studentAnswersDto);
        }

        // api/StudentHomework/answer/by-user/42 
        [HttpGet("by-user/{userId}")]
        [AuthorizeRoles(Role.Methodist, Role.Teacher, Role.Tutor, Role.Student)]
        [Description("Get all answers of student")]
        [ProducesResponseType(typeof(List<StudentHomeworkWithTaskOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<StudentHomeworkWithTaskOutputModel>> GetAllStudentHomeworkByStudentIdAsync(int userId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var answersDto = await _studentHomeworkService.GetAllStudentHomeworkByStudentIdAsync(userId, userInfo);
            return _mapper.Map<List<StudentHomeworkWithTaskOutputModel>>(answersDto);
        }
    }
}