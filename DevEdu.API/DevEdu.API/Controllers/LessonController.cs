using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DevEdu.DAL.Models;
using System;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.Services;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.API.Models.OutputModels.Lesson;
using DevEdu.API.Models.OutputModels;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;        

        public LessonController(IMapper mapper, ILessonService lessonService, ILessonRepository lessonRepository)
        {
            _mapper = mapper;
            _lessonService = lessonService;            
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
        public string DeleteLessonComment(int lessonId, int commentId)
        {
            _lessonService.DeleteCommentFromLesson(lessonId, commentId);
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        [Description("Deletes topic from lesson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonService.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        [Description("Adds topic to lesson")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonService.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        [Description("Adds student to lesson")]
        [ProducesResponseType(typeof(int), (StatusCodes.Status201Created))]
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
            _lessonService.DeleteStudentFromLesson(lessonId, userId); ;
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        [Description("Update Feedback for lesson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public LessonInfoOutputModel UpdateStudentFeedbackForLesson(int lessonId,int userId,  [FromBody] FeedbackInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentFeedbackForLesson(lessonId, userId, dto);
            var output = _lessonService.SelectLessonById(lessonId);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason")]
        [Description("Update AbsenceReason for lesson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public LessonInfoOutputModel UpdateStudentAbsenceReasonOnLesson(int lessonId,int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, dto);
            var output = _lessonService.SelectLessonById(lessonId);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance")]
        [Description("Update Attendance for lesson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public LessonInfoOutputModel UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            _lessonService.UpdateStudentAttendanceOnLesson(lessonId, userId, dto);
            var output = _lessonService.SelectLessonById(lessonId);
            return _mapper.Map<LessonInfoOutputModel>(output);
        }

        // api/lesson/{lessonId}/feedback
        [HttpGet("{lessonId}/feedback")]
        [Description("Get all feedback by lesson")]
        [ProducesResponseType(typeof(FeedbackOutputModel), StatusCodes.Status200OK)]
        public List<FeedbackOutputModel> GetAllFeedbackByLessonId(int lessonId)
        {
            var dto =_lessonService.SelectAllFeedbackByLessonId(lessonId);
            return _mapper.Map<List<FeedbackOutputModel>>(dto);
        }
    }
}