using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ILessonValidationHelper
    {
        LessonDto GetLessonByIdAndThrowIfNotFound(int lessonId);
        public void CheckTopicLessonReferenceIsUnique(LessonDto lesson, int topicId);    
        void CheckUserInLessonAccess(int lessonId, int userId);
    }
}