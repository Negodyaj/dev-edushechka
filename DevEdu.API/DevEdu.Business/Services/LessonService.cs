using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICommentRepository _commentRepository;
        public LessonService(ILessonRepository lessonRepository, ICommentRepository commentRepository)
        {
            _lessonRepository = lessonRepository;
            _commentRepository = commentRepository;
        }
        
        public void AddCommentToLesson(int lessonId, int commentId) => _lessonRepository.AddCommentToLesson(lessonId, commentId);

        public int AddLesson(LessonDto lessonDto) => _lessonRepository.AddLesson(lessonDto);

        public void DeleteCommentFromLesson(int lessonId, int commentId) => _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);

        public void DeleteLesson(int id) => _lessonRepository.DeleteLesson(id);

        public List<LessonDto> SelectAllLessonsByGroupId(int id) => _lessonRepository.SelectAllLessonsByGroupId(id);

        public List<LessonDto> SelectAllLessonsByTeacherId(int id) => _lessonRepository.SelectAllLessonsByTeacherId(id);

        public LessonDto SelectLessonById(int id) => _lessonRepository.SelectLessonById(id);

        public LessonDto SelectLessonWithCommentsById(int id)
        {
            LessonDto result = _lessonRepository.SelectLessonById(id);

            result.Comments = _commentRepository.SelectCommentsFromLessonByLessonId(id);

            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(int id)
        {
            LessonDto result = SelectLessonWithCommentsById(id);

            result.Students = _lessonRepository.SelectStudentsLessonByLessonId(id);

            return result;
        }

        public void UpdateLesson(int id, LessonDto lessonDto)
        {
            lessonDto.Id = id;
            _lessonRepository.UpdateLesson(lessonDto);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId) => 
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);

        public void AddTopicToLesson(int lessonId, int topicId) => 
            _lessonRepository.AddTopicToLesson(lessonId, topicId);

        public void AddStudentToLesson(int lessonId, int userId)
        {
            var studentLessonDto =
               new StudentLessonDto
               {
                   User = new UserDto { Id = userId },
                   Lesson = new LessonDto { Id = lessonId }
               };
            _lessonRepository.AddStudentToLesson(studentLessonDto);
        }

        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            var studentLessonDto =
                new StudentLessonDto
                {
                    User = new UserDto { Id = userId },
                    Lesson = new LessonDto { Id = lessonId }
                };
            _lessonRepository.DeleteStudentFromLesson(studentLessonDto);
        }

        public void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentFeedbackForLesson(studentLessonDto);
        }

        public void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
        }

        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
        }
    }
}