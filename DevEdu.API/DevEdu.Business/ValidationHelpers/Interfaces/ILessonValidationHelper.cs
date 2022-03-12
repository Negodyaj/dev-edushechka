using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        void CheckAttendanceExistence(int lessonId, int userId);
        void CheckTopicLessonReferenceIsUnique(LessonDto lesson, int topicId);
        void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId);
        Task CheckUserBelongsToLessonAsync(int lessonId, int userId);
        Task CheckUserBelongsToLessonAsync(UserIdentityInfo userIdentity, LessonDto lesson);
        Task<LessonDto> GetLessonByIdAndThrowIfNotFoundAsync(int lessonId);
    }
}