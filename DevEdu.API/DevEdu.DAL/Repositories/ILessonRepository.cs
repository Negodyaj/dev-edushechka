using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ILessonRepository
    {
        void AddStudentToLesson(StudentLessonDto studentLessonDto);
        void DeleteStudentFromLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto);
    }
}