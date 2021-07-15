using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ILessonRepository
    {
        void AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessons();
        LessonDto SelectLessonById(int id);
        public void UpdateTeacherCommentOfLesson(LessonDto lessonDto);
        public void UpdateLinkToRecordOfLesson(LessonDto lessonDto);
        int DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto);
    }
}