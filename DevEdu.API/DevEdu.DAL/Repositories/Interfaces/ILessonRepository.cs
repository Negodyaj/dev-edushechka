using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ILessonRepository
    {
        Task<int> AddLessonAsync(LessonDto lessonDto);
        Task DeleteLessonAsync(int id);
        Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(int groupId);
        Task<List<LessonDto>> SelectAllLessonsByTeacherIdAsync(int teacherId);
        Task<LessonDto> SelectLessonByIdAsync(int id);
        Task<List<StudentLessonDto>> SelectStudentsLessonByLessonIdAsync(int lessonId);
        Task UpdateLessonAsync(LessonDto lessonDto);
        Task<int> DeleteTopicFromLessonAsync(int lessonId, int topicId);
        Task AddTopicToLessonAsync(int lessonId, int topicId);
        Task AddStudentToLessonAsync(int lessonId, int userId);
        Task DeleteStudentFromLessonAsync(int lessonId, int userId);
        Task UpdateStudentAbsenceReasonOnLessonAsync(StudentLessonDto studentLessonDto);
        Task UpdateStudentAttendanceOnLessonAsync(StudentLessonDto studentLessonDto);
        Task UpdateStudentFeedbackForLessonAsync(StudentLessonDto studentLessonDto);
        Task<List<StudentLessonDto>> SelectAllFeedbackByLessonIdAsync(int lessonId);
        Task<StudentLessonDto> SelectAttendanceByLessonAndUserIdAsync(int lessonId, int userId);
    }
}