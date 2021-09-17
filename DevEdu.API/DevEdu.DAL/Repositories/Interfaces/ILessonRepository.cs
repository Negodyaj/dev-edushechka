using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ILessonRepository
    {
        int AddLesson(LessonDto lessonDto);
        void DeleteLesson(int id);
        Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(int groupId);
        List<LessonDto> SelectAllLessonsByTeacherId(int teacherId);
        LessonDto SelectLessonById(int id);
        List<StudentLessonDto> SelectStudentsLessonByLessonIdAsync(int lessonId);
        Task UpdateLessonAsync(LessonDto lessonDto);
        int DeleteTopicFromLessonAsync(int lessonId, int topicId);
        void AddTopicToLesson(int lessonId, int topicId);
        void AddStudentToLesson(int lessonId, int userId);
        void DeleteStudentFromLesson(int lessonId, int userId);
        void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto);
        void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto);
        List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId);
        StudentLessonDto SelectAttendanceByLessonAndUserId(int lessonId, int userId);
    }
}