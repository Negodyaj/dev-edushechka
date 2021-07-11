using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ILessonRepository
    {
        int AddCommentToLesson(int lessonId, int commentId);
        int AddLesson(LessonDto lessonDto);
        void AddStudentToLesson(StudentLessonDto studentLessonDto);
        void AddTopicToLesson(int lessonId, int topicId);
        void DeleteCommentFromLesson(int lessonId, int commentId);
        void DeleteLesson(int id);
        void DeleteStudentFromLesson(StudentLessonDto studentLessonDto);
        int DeleteTopicFromLesson(int lessonId, int topicId);
        void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto);
    }
}