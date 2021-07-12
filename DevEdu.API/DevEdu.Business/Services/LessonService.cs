using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;


        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public int AddCommentToLesson(int lessonId, int commentId)
        {
            return _lessonRepository.AddCommentToLesson(lessonId, commentId);
        }

        public int AddLesson(LessonDto lessonDto)
        {
            return _lessonRepository.AddLesson(lessonDto);
        }


        public void DeleteCommentFromLesson(int lessonId, int commentId)
        {
            _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);
        }

        public void DeleteLesson(int id)
        {
            _lessonRepository.DeleteLesson(id);
        }


        public List<LessonDto> SelectAllLessons()
        {
            return _lessonRepository.SelectAllLessons();
        }

        public LessonDto SelectLessonById(int id)
        {
            return _lessonRepository.SelectLessonById(id);
        }

        public int UpdateLesson(int id, String commentTeacher, DateTime date)
        {
            return _lessonRepository.UpdateLesson(id, commentTeacher, date);
        }

        public void DeleteTopicFromLesson(int lessonId, int topicId) => 
            _lessonRepository.DeleteTopicFromLesson(lessonId, topicId);

        public void AddTopicToLesson(int lessonId, int topicId) => 
            _lessonRepository.AddTopicToLesson(lessonId, topicId);
    }
}
