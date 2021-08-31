using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class LessonValidationHelper : ILessonValidationHelper
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IGroupRepository _groupRepository;

        public LessonValidationHelper(
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
                throw new ValidationException(nameof(topicId), string.Format(ServiceMessages.LessonTopicReferenceAlreadyExists, lesson.Id, topicId));
        }

        public void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId)
        {
            if (userIdentity.UserId != teacherId)
                throw new ValidationException(nameof(teacherId), string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.UserId, teacherId));
        }

        public void CheckUserBelongsToLesson(UserIdentityInfo userIdentity, LessonDto lesson)
        {
            if (userIdentity.IsStudent())
            {
                var studentGroups = _groupRepository.GetGroupsByUserId(userIdentity.UserId);
                var result = studentGroups.Where(sg => (lesson.Groups).Any(lg => lg.Id == sg.Id));
                if (result.Count() == 0)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
            else if(userIdentity.IsTeacher())
            {
                if(userIdentity.UserId != lesson.Teacher.Id)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
        }

        public void CheckUserBelongsToLesson(int lessonId, int userId)
        {
            var groupsByLesson = _groupRepository.GetGroupsByLessonId(lessonId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByLesson.Any(gl => gl.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userId, lessonId));
        }
        public void CheckAttendanceExistence(int lessonId, int userId)
        {
            var attandance = _lessonRepository.SelectAttendanceByLessonAndUserId(lessonId, userId);
            if (attandance == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(attandance), lessonId, userId));

        }

    }
}