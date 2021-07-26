using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Models.OutputModels;
using Microsoft.AspNetCore.Authorization;
using DevEdu.DAL.Enums;
using DevEdu.API.Common;
using DevEdu.Business.Services;
using DevEdu.API.Models.OutputModels.Lesson;

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
        [HttpPost]
        [Description("Add a lesson.")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        public int AddLesson([FromBody] LessonInputModel inputModel)
        {
            var dto = _mapper.Map<LessonDto>(inputModel);
            return _lessonService.AddLesson(dto);
        }

        // api/lesson/{id}
        [HttpDelete("{id}")]
        [Description("Delete the lesson by id.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteLesson(int id)
        {
            _lessonService.DeleteLesson(id);
        }

        // api/lesson/{id}
        [HttpPut("{id}")]
        [Description("Update the lesson's teacher comment and link to record.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public LessonInfoOutputModel UpdateLesson(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            _lessonService.UpdateLesson(id, dto);
            var output = _lessonService.SelectLessonById(id);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/groupId/{id}
        [HttpGet("/by-groupId/{id}")]
        [Description("Get all lessons by groupId.")]
        [ProducesResponseType(typeof(List<LessonInfoOutputModel>), StatusCodes.Status200OK)]
        public List<LessonInfoOutputModel> GetAllLessonsByGroupId(int id)
        {
            var dto = _lessonService.SelectAllLessonsByGroupId(id);
            return  _mapper.Map<List<LessonInfoOutputModel>>(dto);
        }

        // api/lesson/teacherId/{id}
        [HttpGet("/by-teacherId/{id}")]
        [Description("Get all lessons by teacherId.")]
        [ProducesResponseType(typeof(List<LessonInfoWithCourseOutputModel>), StatusCodes.Status200OK)]
        public List<LessonInfoWithCourseOutputModel> GetAllLessonsByTeacherId(int id)
        {
            var dto = _lessonService.SelectAllLessonsByTeacherId(id);
            return _mapper.Map<List<LessonInfoWithCourseOutputModel>>(dto);
        }

        // api/lesson/{id}
        [HttpGet("{id}")]
        [Description("Get the lesson by id.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status200OK)]
        public LessonInfoOutputModel GetLessonById(int id)
        {
            var dto = _lessonService.SelectLessonById(id);
            return _mapper.Map<LessonInfoOutputModel>(dto);
        }

        // api/lesson/{id}/with-comments
        [HttpGet("{id}/with-comments")]
        [Description("Get the lesson with comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithCommentsOutputModel), StatusCodes.Status200OK)]
        public LessonInfoWithCommentsOutputModel GetAllLessonsWithComments(int id)
        {
            var dto = _lessonService.SelectLessonWithCommentsById(id);
            return _mapper.Map<LessonInfoWithCommentsOutputModel>(dto);
        }

        // api/lesson/{id}/full-info"
        [AuthorizeRoles(Role.Student, Role.Teacher)]
        [HttpGet("{id}/full-info")]
        [Description("Get the lesson with students and comments by id.")]
        [ProducesResponseType(typeof(LessonInfoWithStudentsAndCommentsOutputModel), StatusCodes.Status200OK)]
        public LessonInfoWithStudentsAndCommentsOutputModel GetAllLessonsWithStudentsAndComments(int id)
        {
            var dto = _lessonService.SelectLessonWithCommentsAndStudentsById(id);
            return _mapper.Map<LessonInfoWithStudentsAndCommentsOutputModel> (dto);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        [Description("Delete topic from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonService.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        [Description("Add topic to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonService.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        [Description("Adds student to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddStudentToLesson(int lessonId, int userId )
        {
            _lessonService.AddStudentToLesson(lessonId, userId);
        }


        // api/lesson/{lessonId}/user/{userId}
        [HttpDelete("{lessonId}/user/{userId}")]
        [Description("Deletes student from lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            _lessonService.DeleteStudentFromLesson(lessonId, userId); 
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [AuthorizeRoles(Role.Student)]
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        [Description("Update Feedback for lesson")]
        [ProducesResponseType(typeof(StudentLessonShortOutputModel), StatusCodes.Status200OK)]
        public StudentLessonShortOutputModel UpdateStudentFeedbackForLesson(int lessonId,int userId, [FromBody] FeedbackInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentFeedbackForLesson(lessonId, userId, dto);
            return GetStudenLessonByLessonAndUserId(lessonId, userId);
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason")]
        [Description("Update AbsenceReason for lesson")]
        [ProducesResponseType(typeof(StudentLessonShortOutputModel), StatusCodes.Status200OK)]
        public StudentLessonShortOutputModel UpdateStudentAbsenceReasonOnLesson(int lessonId,int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, dto);
            return GetStudenLessonByLessonAndUserId(lessonId, userId);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance")]
        [Description("Update Attendance for lesson")]
        [ProducesResponseType(typeof(StudentLessonShortOutputModel),StatusCodes.Status200OK)]
        public StudentLessonShortOutputModel UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAttendanceOnLesson(lessonId, userId, dto);            
            return GetStudenLessonByLessonAndUserId(lessonId, userId);
        }

        // api/lesson/{lessonId}/feedback
        [HttpGet("{lessonId}/feedback")]
        [Description("Get all feedback by lesson")]
        [ProducesResponseType(typeof(List<FeedbackOutputModel>), StatusCodes.Status200OK)]
        public List<FeedbackOutputModel> GetAllFeedbackByLessonId(int lessonId)
        {
            var dto =_lessonService.SelectAllFeedbackByLessonId(lessonId);
            return _mapper.Map<List<FeedbackOutputModel>>(dto);
        }
        // api/lesson/{lessonId}/user/{userId}
        [HttpGet("{lessonId}/user/{userId}")]
        [Description("Get StudenLesson by lesson and user")]
        [ProducesResponseType(typeof(StudentLessonShortOutputModel), StatusCodes.Status200OK)]
        public StudentLessonShortOutputModel GetStudenLessonByLessonAndUserId(int lessonId, int userId)
        {
            var output =_lessonService.GetStudenLessonByLessonAndUserId(lessonId, userId);
            return _mapper.Map<StudentLessonShortOutputModel>(output);
        }
    }
}