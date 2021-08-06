namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        void CheckLessonExistence(int lessonId);
        void CheckUserInLessonAccess(int lessonId, int userId);
        void CheckAttendanceExistence(int lessonId, int userId);
    }
}