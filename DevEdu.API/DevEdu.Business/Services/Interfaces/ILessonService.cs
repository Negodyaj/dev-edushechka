﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        int AddLesson(LessonDto lessonDto, List<int> topicIds);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessonsByGroupId(int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonById(int id);
        LessonDto SelectLessonWithCommentsById(int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(int id);
        LessonDto UpdateLesson(LessonDto lessonDto, int id);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId);
        StudentLessonDto GetStudentLessonByLessonAndUserId(int lessonId, int userId);
    }
}