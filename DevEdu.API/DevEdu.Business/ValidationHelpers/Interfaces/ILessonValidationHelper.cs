namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        void CheckLessonExistence(int lessonId);
        public void CheckTopicLessonReferenceIsUnique(int lessonId, int topicId);    
    }
}