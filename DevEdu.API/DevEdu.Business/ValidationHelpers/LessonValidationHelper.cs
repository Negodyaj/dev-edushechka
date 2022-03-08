using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<LessonDto> GetLessonByIdAndThrowIfNotFoundAsync(int lessonId)
        {
            var lesson = await _lessonRepository.SelectLessonByIdAsync(lessonId);
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

        public async System.Threading.Tasks.Task CheckUserBelongsToLessonAsync(UserIdentityInfo userIdentity, LessonDto lesson)
        {
            if (userIdentity.IsStudent())
            {
                var studentGroups = await _groupRepository.GetGroupsByUserIdAsync(userIdentity.UserId);
                var result = studentGroups.Where(sg => (lesson.Groups).Any(lg => lg.Id == sg.Id));
                if (!result.Any())
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
            else if (userIdentity.IsTeacher())
            {
                if (userIdentity.UserId != lesson.Teacher.Id)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
        }

        public async Task CheckUserBelongsToLessonAsync(int lessonId, int userId)
        {
            var groupsByLesson = await _groupRepository.GetGroupsByLessonIdAsync(lessonId);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);
            var result = groupsByUser.FirstOrDefault(gu => groupsByLesson.Any(gl => gl.Id == gu.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userId, lessonId));
        }

        public void CheckAttendanceExistence(int lessonId, int userId)
        {
            var attendance = _lessonRepository.SelectAttendanceByLessonAndUserIdAsync(lessonId, userId);
            if (attendance == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(attendance), lessonId));
        }

        public async Task CheckAccessAddStudentsAtGroupToLessonAsync(int groupId, int leadId, List<Role> roles)
        {
            var groupDto = await _groupRepository.GetGroupAsync(groupId);

            bool isAccess = roles.Where(r => r == Role.Admin || r == Role.Manager).Any();
            bool isTeacherAtGroup = groupDto.Teachers.Where(t => t.Id == leadId).Any();

            if (!isTeacherAtGroup && !isAccess)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessAddAttendanceExistence, leadId));

        }
    }
}