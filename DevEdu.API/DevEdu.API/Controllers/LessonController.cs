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
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public LessonController
        (
            ILessonService lessonService,
            IMapper mapper
        )
        {
            _lessonService = lessonService;
            _mapper = mapper;
            _lessonService = lessonService;
        }

        // api/lesson
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPost]
        [Description("Add a lesson")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<LessonInfoOutputModel>> AddLessonAsync([FromBody] LessonInputModel inputModel)
        {
            var lessonDto = _mapper.Map<LessonDto>(inputModel);
            var userIdentity = this.GetUserIdAndRoles();
            var addedLesson = await _lessonService.AddLessonAsync(userIdentity, lessonDto, inputModel.TopicIds);
            var output = _mapper.Map<LessonInfoOutputModel>(addedLesson);
            return Created(new Uri($"api/Lesson/{output.Id}", UriKind.Relative), output);
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete the lesson by id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteLessonAsync(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            await _lessonService.DeleteLessonAsync(userIdentity, id);
            return NoContent();
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPut("{id}")]
        [Description("Update the lesson's teacher comment and link to record.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<LessonInfoOutputModel> UpdateLessonAsync(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            var userIdentity = this.GetUserIdAndRoles();
            var output = await _lessonService.UpdateLessonAsync(userIdentity, dto, id);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/groupId/{id}
        [AuthorizeRoles(Role.Teacher, Role.Student)]
        [HttpGet("/by-groupId/{id}")]
        [Description("Get all lessons by groupId.")]
        [ProducesResponseType(typeof(List<LessonInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<LessonInfoOutputModel>> GetAllLessonsByGroupIdAsync(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = await _lessonService.SelectAllLessonsByGroupIdAsync(userIdentity, id);
            return _mapper.Map<List<LessonInfoOutputModel>>(dto);
        }

        // api/lesson/teacherId/{id}
        [AuthorizeRoles(Role.Manager, Role.Methodist)]
        [HttpGet("/by-teacherId/{id}")]
        [Description("Get all lessons by teacherId.")]
        [ProducesResponseType(typeof(List<LessonInfoWithCourseOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<LessonInfoWithCourseOutputModel>> GetAllLessonsByTeacherIdAsync(int id)
        {
            var dto = await _lessonService.SelectAllLessonsByTeacherIdAsync(id);
            return _mapper.Map<List<LessonInfoWithCourseOutputModel>>(dto);
        }

        // api/lesson/{id}/with-comments
        [AuthorizeRolesAttribute(Role.Student)]
        [HttpGet("{id}/with-comments")]
        [Description("Get the lesson with comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithCommentsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<LessonInfoWithCommentsOutputModel> GetLessonWithCommentsAsync(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = await _lessonService.SelectLessonWithCommentsByIdAsync(userIdentity, id);
            return _mapper.Map<LessonInfoWithCommentsOutputModel>(dto);
        }

        // api/lesson/{id}/full-info
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpGet("{id}/full-info")]
        [Description("Get the lesson with students and comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithStudentsAndCommentsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<LessonInfoWithStudentsAndCommentsOutputModel> GetLessonWithStudentsAndCommentsAsync(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = await _lessonService.SelectLessonWithCommentsAndStudentsByIdAsync(userIdentity, id);
            return _mapper.Map<LessonInfoWithStudentsAndCommentsOutputModel>(dto);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{lessonId}/topic/{topicId}")]
        [Description("Delete topic from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTopicFromLessonAsync(int lessonId, int topicId)
        {
            await _lessonService.DeleteTopicFromLessonAsync(lessonId, topicId);
            return NoContent();
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("{lessonId}/topic/{topicId}")]
        [Description("Add topic to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddTopicToLessonAsync(int lessonId, int topicId)
        {
            await _lessonService.AddTopicToLessonAsync(lessonId, topicId);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        // api/lesson/{lessonId}/student/{studentId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("{lessonId}/student/{studentId}")]
        [Description("Adds student to lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentLessonOutputModel>> AddStudentToLessonAsync(int lessonId, int studentId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = await _lessonService.AddStudentToLessonAsync(lessonId, studentId, userInfo);
            var output = _mapper.Map<StudentLessonOutputModel>(dto);
            return Created(new Uri($"api/Lesson/{output.Id}/full-info", UriKind.Relative), output);
        }

        // api/lesson/{lessonId}/student/{studentId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{lessonId}/student/{studentId}")]
        [Description("Deletes student from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudentFromLessonAsync(int lessonId, int studentId)
        {
            var userInfo = this.GetUserIdAndRoles();
            await _lessonService.DeleteStudentFromLessonAsync(lessonId, studentId, userInfo);
            return NoContent();
        }

        // api/lesson/{lessonId}/student/{studentId}/feedback
        [AuthorizeRoles(Role.Student)]
        [HttpPut("{lessonId}/student/{studentId}/feedback")]
        [Description("Update Feedback for lesson")]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public async Task<StudentLessonOutputModel> UpdateStudentFeedbackForLessonAsync(int lessonId, int studentId, [FromBody] FeedbackInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = await _lessonService.UpdateStudentFeedbackForLessonAsync(lessonId, studentId, dto, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/student/{studentId}/absenceReason
        [AuthorizeRoles(Role.Student)]
        [HttpPut("{lessonId}/student/{studentId}/absenceReason")]
        [Description("Update AbsenceReason for lesson")]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public async Task<StudentLessonOutputModel> UpdateStudentAbsenceReasonOnLessonAsync(int lessonId, int studentId, [FromBody] AbsenceReasonInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = await _lessonService.UpdateStudentAbsenceReasonOnLessonAsync(lessonId, studentId, dto, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/student/{studentId}/attendance
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{lessonId}/student/{studentId}/attendance")]
        [Description("Update Attendance for lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public async Task<StudentLessonOutputModel> UpdateStudentAttendanceOnLessonAsync(int lessonId, int studentId, [FromBody] AttendanceInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = await _lessonService.UpdateStudentAttendanceOnLessonAsync(lessonId, studentId, dto, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/feedback
        [AuthorizeRoles(Role.Teacher, Role.Manager)]
        [HttpGet("{lessonId}/feedback")]
        [Description("Get all feedback by lesson")]
        [ProducesResponseType(typeof(List<FeedbackOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<List<FeedbackOutputModel>> GetAllFeedbackByLessonIdAsync(int lessonId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = await _lessonService.SelectAllFeedbackByLessonIdAsync(lessonId, userInfo);
            return _mapper.Map<List<FeedbackOutputModel>>(dto);
        }
    }
}