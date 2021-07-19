﻿using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        void AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessons();
        LessonDto SelectLessonById(int id);
        void UpdateLesson(int id, LessonDto lessonDto);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
    }
}