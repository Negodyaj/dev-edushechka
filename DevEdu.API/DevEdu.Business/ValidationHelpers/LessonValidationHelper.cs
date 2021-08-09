using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class LessonValidationHelper : ILessonValidationHelper
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IGroupRepository _groupRepository;

        public LessonValidationHelper
        (
            ILessonRepository lessonRepository,
            IGroupRepository groupRepository
        )
        {
            _lessonRepository = lessonRepository;
            _groupRepository = groupRepository;
        }

        public LessonDto GetLessonByIdAndThrowIfNotFound(int lessonId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lessonId));
            return lesson;
        }

        public void CheckTopicLessonReferenceIsUnique(LessonDto lesson, int topicId)
        {
            if (lesson.Topics.Any(topic => topic.Id == topicId))
                throw new ValidationException(string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lesson.Id, topicId));
        }

        public void CheckUserInLessonAccess(int lessonId, int userId)
        {
            var groupsByLesson = _groupRepository.GetGroupsByLessonId(lessonId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByLesson.Any(gl => gl.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, lessonId));
        }
    }
}