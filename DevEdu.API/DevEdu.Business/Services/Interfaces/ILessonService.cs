using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ILessonService
    {
        Task<LessonDto> AddLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, List<int> topicIds);
        Task DeleteLessonAsync(UserIdentityInfo userIdentity, int id);
        Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(UserIdentityInfo userIdentity, int id);
        Task<List<LessonDto>> SelectAllLessonsByTeacherIdAsync(int id);
        Task<LessonDto> SelectLessonWithCommentsByIdAsync(UserIdentityInfo userIdentity, int id);
        Task<LessonDto> SelectLessonWithCommentsAndStudentsByIdAsync(UserIdentityInfo userIdentity, int id);
        Task<LessonDto> UpdateLessonAsync(UserIdentityInfo userIdentity, LessonDto lessonDto, int id);
        Task DeleteTopicFromLessonAsync(int lessonId, int topicId);
        Task AddTopicToLessonAsync(int lessonId, int topicId);
        Task<StudentLessonDto> AddStudentToLessonAsync(int lessonId, int userId, UserIdentityInfo userIdentityInfo);
        Task DeleteStudentFromLessonAsync(int lessonId, int userId, UserIdentityInfo userIdentityInfo);
        Task<StudentLessonDto> UpdateStudentAbsenceReasonOnLessonAsync(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        Task<StudentLessonDto> UpdateStudentAttendanceOnLessonAsync(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        Task<StudentLessonDto> UpdateStudentFeedbackForLessonAsync(int lessonId, int userId, StudentLessonDto studentLessonDto, UserIdentityInfo userIdentityInfo);
        Task<List<StudentLessonDto>> SelectAllFeedbackByLessonIdAsync(int lessonId, UserIdentityInfo userIdentityInfo);
    }
}