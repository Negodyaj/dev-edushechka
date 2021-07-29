namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        void CheckLessonExistence(int lessonId);
        void CheckUserInLessonExistence(int lessonId, int userId);
    }
}