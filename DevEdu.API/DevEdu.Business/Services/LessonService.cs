using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
        
        public void AddCommentToLesson(int lessonId, int commentId) => _lessonRepository.AddCommentToLesson(lessonId, commentId);

        public int AddLesson(LessonDto lessonDto) => _lessonRepository.AddLesson(lessonDto);

        public void DeleteCommentFromLesson(int lessonId, int commentId) => _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);

        public void DeleteLesson(int id) => _lessonRepository.DeleteLesson(id);

        public List<LessonDto> SelectAllLessons() => _lessonRepository.SelectAllLessons();

        public LessonDto SelectLessonById(int id) => _lessonRepository.SelectLessonById(id);

        public void UpdateLesson(int id, LessonDto lessonDto)
        {
            lessonDto.Id = id;
            _lessonRepository.UpdateLesson(lessonDto);
        }
        public void DeleteTopicFromLesson(int lessonId, int topicId) => 
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);

        public void AddTopicToLesson(int lessonId, int topicId) => 
            _lessonRepository.AddTopicToLesson(lessonId, topicId);
    }
}