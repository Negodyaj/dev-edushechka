using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class LessonValidationHelper : ILessonValidationHelper
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonValidationHelper(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public void CheckLessonExistence(int lessonId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lessonId));
        }
        public void CheckUserInLessonExistence(int lessonId, int userId)
        {
            var studentLesson = _lessonRepository.SelectByLessonAndUserId(lessonId, userId);
            if (studentLesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, lessonId));
        }
    }
}