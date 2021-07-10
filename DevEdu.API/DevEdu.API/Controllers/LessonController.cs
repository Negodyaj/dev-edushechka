using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonService _lessonService;

        public LessonController(IMapper mapper, ILessonRepository lessonRepository, ILessonService lessonService)
        {
            _lessonRepository = lessonRepository;
            _lessonService = lessonService;
            
            _mapper = mapper;
        }

        // api/lesson
        [HttpPost]
        public string AddLesson([FromBody] LessonInputModel inputModel)
        {
            return $"Date {inputModel.Date} TeacherComment {inputModel.TeacherComment}  TeacherId {inputModel.TeacherId}";
        }

        // api/lesson/{id}
        [HttpDelete("{id}")]
        public string DeleteLesson(int id)
        {
            return $"id {id}";
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpPost("{lessonId}/comment/{commentId}")]
        public string AddLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpDelete("{lessonId}/comment/{commentId}")]
        public string DeleteLessonComment(int lessonId, int commentId)
        {
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        public string DeleteTopicFromLesson(int lessonId, int topicId)
        {
            return $"lessonId {lessonId} topicId {topicId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        public string AddTopicToLesson(int lessonId, int topicId)
        {
            return $"lessonId {lessonId} topicId {topicId}";
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        public void AddStudenToLesson(int lessonId, int userId)
        {
            _lessonService.AddStudentToLesson(lessonId, userId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpDelete("{lessonId}/user/{userId}")]
        public void DeleteStudentFromLesson(int lessonId, int userId)
        {        
            _lessonService.DeleteStudentFromLesson(lessonId, userId); 
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        public void UpdateStudentFeedbackForLesson(int lessonId, int userId, [FromBody] FeedbackInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);          
            _lessonService.UpdateStudentFeedbackForLesson(lessonId, userId,dto);
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason ")]
        public void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);          
            _lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, dto);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance ")]
        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);           
            _lessonService.UpdateStudentAttendanceOnLesson(lessonId, userId, dto);
        }
    }
}