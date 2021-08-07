using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        LessonDto CheckLessonExistence(int lessonId);
        void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId);
        void CheckUserBelongsToLesson(UserIdentityInfo userIdentity, LessonDto lesson);
        void CheckUserInLessonAccess(int lessonId, int userId);
    }
}