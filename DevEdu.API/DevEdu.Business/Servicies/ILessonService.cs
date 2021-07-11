using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        int AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
    }
}