using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        Task<LessonDto> AddLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds);
        Task<StudentLessonDto> AddStudentToLessonAsync(int lessonId, int studentId, UserIdentityInfo userIdentityInfo);
        Task AddTopicToLessonAsync(int lessonId, int topicId);
        Task<List<StudentLessonDto>> AddVisitsStudentsAtGroupToLessonAsync(UserIdentityInfo userIdentity, int lessonId, int groupId);
        Task DeleteLessonAsync(UserIdentityInfo userIdentity, int id);
        Task DeleteStudentFromLessonAsync(int lessonId, int studentId, UserIdentityInfo userIdentityInfo);
        Task DeleteTopicFromLessonAsync(int lessonId, int topicId);
        Task<List<StudentLessonDto>> SelectAllFeedbackByLessonIdAsync(int lessonId, UserIdentityInfo userIdentityInfo);
        Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(UserIdentityInfo userIdentity, int groupId);
        Task<List<LessonDto>> SelectAllLessonsByTeacherIdAsync(int teacherId);
        Task<LessonDto> SelectLessonWithCommentsAndStudentsByIdAsync(UserIdentityInfo userIdentity, int id);
        Task<LessonDto> SelectLessonWithCommentsByIdAsync(UserIdentityInfo userIdentity, int id);
        Task<LessonDto> UpdateLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, int lessonId);
        Task<StudentLessonDto> UpdateStudentAbsenceReasonOnLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        Task<StudentLessonDto> UpdateStudentAttendanceOnLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        Task<StudentLessonDto> UpdateStudentFeedbackForLessonAsync(int lessonId, int studentId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
    }
}