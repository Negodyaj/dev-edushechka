using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;
using System.Linq;

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

        public void CheckTopicLessonReferenceIsUnique(int lessonId, int topicId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson.Topics.Any(topic => topic.Id == topicId))
                throw new ValidationException(string.Format(ServiceMessages.SameTopicsInLesson, lessonId, topicId));
        }
    }
}