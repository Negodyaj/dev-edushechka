using AutoMapper;
using DevEdu.API.Models.InputModels;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly ILessonRepository _lessonRepository;

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
            var dto = _mapper.Map<LessonDto>(inputModel);
            return _lessonService.AddLesson(dto).ToString();
        }

        // api/lesson/{id}
        [HttpDelete("{id}")]
        public void DeleteLesson(int id)
        {
            _lessonService.DeleteLesson(id);
        }

        // api/lesson/{id}
        [HttpPut("{id}")]
        public void UpdateLesson(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            _lessonService.UpdateLesson(id, dto);
        }

        // api/lesson/{id}
        [HttpGet("{id}")]
        public LessonDto GetLessonById(int id)
        {
            return _lessonService.SelectLessonById(id);
        }

        // api/lesson
        [HttpGet]
        public List<LessonDto> GetAllLessons()
        {
            return _lessonService.SelectAllLessons();
        }


        // api/lesson/{lessonId}/comment/{commentId}
        [HttpPost("{lessonId}/comment/{commentId}")]
        public void AddLessonComment(int lessonId, int commentId)
        {
            _lessonService.AddCommentToLesson(lessonId, commentId);
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpDelete("{lessonId}/comment/{commentId}")]
        public void DeleteLessonComment(int lessonId, int commentId)
        {
            _lessonService.DeleteCommentFromLesson(lessonId, commentId);
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonRepository.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        public void AddStudentToLesson(int lessonId, int userId)
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
            _lessonService.UpdateStudentFeedbackForLesson(lessonId, userId, dto);
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason")]
        public void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, dto);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance")]
        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAttendanceOnLesson(lessonId, userId, dto);
        }
    }
}