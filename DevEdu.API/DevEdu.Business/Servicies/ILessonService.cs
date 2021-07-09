using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public interface ILessonService
    {
        int AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessons();
        LessonDto SelectLessonById(int id);
        int UpdateLesson(int id, string commentDto, DateTime date);
    }
}