using AutoMapper;
using DevEdu.API.Common;
using DevEdu.API.Configuration;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.API.Models.OutputModels.Lesson;
using DevEdu.Business.Services;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using DevEdu.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentService _commentService;

        public LessonController
        (
            ILessonRepository lessonRepository,
            ILessonService lessonService,
            ICommentService commentService,
            IMapper mapper
        )
        {
            _lessonRepository = lessonRepository;
            _lessonService = lessonService;
            _commentService = commentService;
            _mapper = mapper;
            _lessonService = lessonService;
        }

        // api/lesson
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPost]
        [Description("Add a lesson.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public LessonInfoOutputModel AddLesson([FromBody] LessonInputModel inputModel)
        {
            var lessonDto = _mapper.Map<LessonDto>(inputModel);
            var userIdentity = this.GetUserIdAndRoles();
            var output = _lessonService.AddLesson(userIdentity, lessonDto, inputModel.TopicIds);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpDelete("{id}")]
        [Description("Delete the lesson by id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteLesson(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            _lessonService.DeleteLesson(userIdentity, id);
        }

        // api/lesson/{id}
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpPut("{id}")]
        [Description("Update the lesson's teacher comment and link to record.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public LessonInfoOutputModel UpdateLesson(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            var userIdentity = this.GetUserIdAndRoles();
            var output = _lessonService.UpdateLesson(userIdentity, dto, id);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/groupId/{id}
        [AuthorizeRolesAttribute(Role.Teacher, Role.Student)]
        [HttpGet("/by-groupId/{id}")]
        [Description("Get all lessons by groupId.")]
        [ProducesResponseType(typeof(List<LessonInfoOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<LessonInfoOutputModel> GetAllLessonsByGroupId(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = _lessonService.SelectAllLessonsByGroupId(userIdentity, id);
            return  _mapper.Map<List<LessonInfoOutputModel>>(dto);
        }

        // api/lesson/teacherId/{id}
        [AuthorizeRolesAttribute(Role.Manager, Role.Methodist)]
        [HttpGet("/by-teacherId/{id}")]
        [Description("Get all lessons by teacherId.")]
        [ProducesResponseType(typeof(List<LessonInfoWithCourseOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<LessonInfoWithCourseOutputModel> GetAllLessonsByTeacherId(int id)
        {
            var dto = _lessonService.SelectAllLessonsByTeacherId(id);
            return _mapper.Map<List<LessonInfoWithCourseOutputModel>>(dto);
        }
              
        // api/lesson/{id}/with-comments
        [AuthorizeRolesAttribute(Role.Student)]
        [HttpGet("{id}/with-comments")]
        [Description("Get the lesson with comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithCommentsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public LessonInfoWithCommentsOutputModel GetAllLessonsWithComments(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = _lessonService.SelectLessonWithCommentsById(userIdentity, id);
            return _mapper.Map<LessonInfoWithCommentsOutputModel>(dto);
        }

        // api/lesson/{id}/full-info"
        [AuthorizeRolesAttribute(Role.Teacher)]
        [HttpGet("{id}/full-info")]
        [Description("Get the lesson with students and comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithStudentsAndCommentsOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public LessonInfoWithStudentsAndCommentsOutputModel GetAllLessonsWithStudentsAndComments(int id)
        {
            var userIdentity = this.GetUserIdAndRoles();
            var dto = _lessonService.SelectLessonWithCommentsAndStudentsById(userIdentity, id);
            return _mapper.Map<LessonInfoWithStudentsAndCommentsOutputModel> (dto);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        [Description("Delete topic from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonService.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        [Description("Add topic to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonService.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/student/{studentId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpPost("{lessonId}/student/{studentId}")]
        [Description("Adds student to lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status204NoContent)]
        public StudentLessonOutputModel AddStudentToLesson(int lessonId, int studentId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var output = _lessonService.AddStudentToLesson(lessonId, studentId, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }


        // api/lesson/{lessonId}/student/{studentId}
        [AuthorizeRoles(Role.Teacher)]
        [HttpDelete("{lessonId}/student/{studentId}")]
        [Description("Deletes student from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteStudentFromLesson(int lessonId, int studentId)
        {
            var userInfo = this.GetUserIdAndRoles();
            _lessonService.DeleteStudentFromLesson(lessonId, studentId, userInfo);
        }

        // api/lesson/{lessonId}/student/{studentId}/feedback
        [AuthorizeRoles(Role.Student)]
        [HttpPut("{lessonId}/student/{studentId}/feedback")]
        [Description("Update Feedback for lesson")]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(StudentLessonOutputModel), StatusCodes.Status200OK)]
        public StudentLessonOutputModel UpdateStudentFeedbackForLesson(int lessonId, int studentId, [FromBody] FeedbackInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = _lessonService.UpdateStudentFeedbackForLesson(lessonId, studentId, dto, userInfo);
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
        public StudentLessonOutputModel UpdateStudentAbsenceReasonOnLesson(int lessonId, int studentId, [FromBody] AbsenceReasonInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = _lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, studentId, dto, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/student/{studentId}/attendance
        [AuthorizeRoles(Role.Teacher)]
        [HttpPut("{lessonId}/student/{studentId}/attendance")]
        [Description("Update Attendance for lesson")]
        [ProducesResponseType(typeof(StudentLessonOutputModel),StatusCodes.Status200OK)]
        public StudentLessonOutputModel UpdateStudentAttendanceOnLesson(int lessonId, int studentId, [FromBody] AttendanceInputModel model)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _mapper.Map<StudentLessonDto>(model);
            var output = _lessonService.UpdateStudentAttendanceOnLesson(lessonId, studentId, dto, userInfo);
            return _mapper.Map<StudentLessonOutputModel>(output);
        }

        // api/lesson/{lessonId}/feedback
        [AuthorizeRoles(Role.Teacher, Role.Manager)]
        [HttpGet("{lessonId}/feedback")]
        [Description("Get all feedback by lesson")]
        [ProducesResponseType(typeof(List<FeedbackOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public List<FeedbackOutputModel> GetAllFeedbackByLessonId(int lessonId)
        {
            var userInfo = this.GetUserIdAndRoles();
            var dto = _lessonService.SelectAllFeedbackByLessonId(lessonId, userInfo);
            return _mapper.Map<List<FeedbackOutputModel>>(dto);
        }
    }
}