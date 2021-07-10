using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto);
    }
}