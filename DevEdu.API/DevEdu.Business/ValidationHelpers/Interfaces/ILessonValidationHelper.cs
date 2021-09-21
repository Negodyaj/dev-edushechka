using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        Task<LessonDto> GetLessonByIdAndThrowIfNotFoundAsync(int lessonId);
        public void CheckTopicLessonReferenceIsUnique(LessonDto lesson, int topicId);
        void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId);
        Task CheckUserBelongsToLessonAsync(UserIdentityInfo userIdentity, LessonDto lesson);
        Task CheckUserBelongsToLessonAsync(int lessonId, int userId);
        void CheckAttendanceExistence(int lessonId, int userId);
    }
}