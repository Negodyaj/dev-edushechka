using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        void AddCommentToLesson(int lessonId, CommentDto commentDto);
        int AddLesson(LessonDto lessonDto, List<int> topicIds);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        List<LessonDto> SelectAllLessonsByGroupId(int id);
        List<LessonDto> SelectAllLessonsByTeacherId(int id);
        LessonDto SelectLessonById(int id);
        LessonDto SelectLessonWithCommentsById(int id);
        LessonDto SelectLessonWithCommentsAndStudentsById(int id);
        LessonDto UpdateLesson(LessonDto lessonDto, int id);
        void DeleteTopicFromLesson(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        StudentLessonDto AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        StudentLessonDto UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        StudentLessonDto UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        StudentLessonDto UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId);        
    }
}