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
using DevEdu.API.Models.OutputModels;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        private readonly ILessonRepository _lessonRepository;

        public LessonController(IMapper mapper, ILessonService lessonService, ILessonRepository lessonRepository)
        {
            _mapper = mapper;
            _lessonService = lessonService;
            _lessonRepository = lessonRepository;
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
        [Description("Update the lesson by id.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void UpdateLesson(int id, [FromBody] LessonUpdateInputModel updateModel)
        {
            var dto = _mapper.Map<LessonDto>(updateModel);
            _lessonService.UpdateLesson(id, dto);
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

        // api/lesson
        [HttpGet]
        [Description("Get all lessons.")]
        [ProducesResponseType(typeof(LessonInfoOutputModel), StatusCodes.Status200OK)]
        public List<LessonInfoOutputModel> GetAllLessons()
        {
            var dto = _lessonService.SelectAllLessons();
            return  _mapper.Map<List<LessonInfoOutputModel>>(dto);
        }

        // api/lesson
        [HttpGet]
        [Description("Get all lessons with topics.")]
        [ProducesResponseType(typeof(LessonInfoWithTopicsOutputModel), StatusCodes.Status200OK)]
        public List<LessonInfoWithTopicsOutputModel> GetAllLessonsWithTopics()
        {
            var dto = _lessonService.SelectAllLessons();
            return _mapper.Map<List<LessonInfoWithTopicsOutputModel>>(dto);
        }

        // api/lesson
        [HttpGet]
        [Description("Get all lessons with groups.")]
        [ProducesResponseType(typeof(LessonInfoWithGroupsOutputModel), StatusCodes.Status200OK)]
        public List<LessonInfoWithGroupsOutputModel> GetAllLessonsWithGroups()
        {
            var dto = _lessonService.SelectAllLessons();
            return _mapper.Map<List<LessonInfoWithGroupsOutputModel>>(dto);
        }

        // api/lesson
        [HttpGet]
        [Description("Get all lessons with students and comments.")]
        [ProducesResponseType(typeof(LessonInfoWithStudentsAndCommentsOutputModel), StatusCodes.Status200OK)]
        public List<LessonInfoWithStudentsAndCommentsOutputModel> GetAllLessonsWithStudentsAndComments()
        {
            var dto = _lessonService.SelectAllLessons();
            return _mapper.Map<List<LessonInfoWithStudentsAndCommentsOutputModel>>(dto);
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpPost("{lessonId}/comment/{commentId}")]
        [Description("Add a lesson's comment.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void AddLessonComment(int lessonId, int commentId)
        {
            _lessonService.AddCommentToLesson(lessonId, commentId);
        }

        // api/lesson/{lessonId}/comment/{commentId}
        [HttpDelete("{lessonId}/comment/{commentId}")]
        [Description("Delete the lesson's comment.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public string DeleteLessonComment(int lessonId, int commentId)
        {
            _lessonService.DeleteCommentFromLesson(lessonId, commentId);
            return $"lessonId {lessonId} commentId {commentId}";
        }

        // api/lesson/{lessonId}/topic/{toppicId}
        [HttpDelete("{lessonId}/topic/{topicId}")]
        public void DeleteTopicFromLesson(int lessonId, int topicId)
        {
            _lessonService.DeleteTopicFromLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/topic/{topicId}
        [HttpPost("{lessonId}/topic/{topicId}")]
        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _lessonService.AddTopicToLesson(lessonId, topicId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpPost("{lessonId}/user/{userId}")]
        public void AddStudentToLesson(int lessonId, int userId )
        {
            _lessonRepository.AddStudentToLesson(lessonId, userId);
        }

        // api/lesson/{lessonId}/user/{userId}
        [HttpDelete("{lessonId}/user/{userId}")]
        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            _lessonRepository.DeleteStudentFromLesson(lessonId, userId); ;
        }

        // api/lesson/{lessonId}/user/{userId}/feedback
        [HttpPut("{lessonId}/user/{userId}/feedback")]
        public void UpdateStudentFeedbackForLesson(int lessonId,int userId,  [FromBody] FeedbackInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            dto.LessonId = lessonId;
            dto.UserId = userId;
            _lessonRepository.UpdateStudentFeedbackForLesson(dto); 
        }

        // api/lesson/{lessonId}/user/{userId}/absenceReason
        [HttpPut("{lessonId}/user/{userId}/absenceReason ")]
        public void UpdateStudentAbsenceReasonOnLesson(int lessonId,int userId, [FromBody] AbsenceReasonInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            dto.LessonId = lessonId;
            dto.UserId = userId;
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(dto);
        }

        // api/lesson/{lessonId}/user/{userId}/attendance
        [HttpPut("{lessonId}/user/{userId}/attendance ")] 
        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, [FromBody] AttendanceInputModel model)
        {
            var dto = _mapper.Map<StudentLessonDto>(model);
            dto.LessonId = lessonId;
            dto.UserId = userId;
            _lessonRepository.UpdateStudentAttendanceOnLesson(dto); 
        }
    }
}