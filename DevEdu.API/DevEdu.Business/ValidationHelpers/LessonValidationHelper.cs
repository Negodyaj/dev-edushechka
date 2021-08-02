using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class LessonValidationHelper : ILessonValidationHelper
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public LessonValidationHelper(
            ILessonRepository lessonRepository,
            IGroupRepository groupRepository,
            IUserRepository userRepository
        )
        {
            _lessonRepository = lessonRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            
        }

        public void CheckLessonExistence(int lessonId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lessonId));
        }

        public void CheckTeacherExistence(int teacherId)
        {
            var user = _userRepository.SelectUserById(teacherId);
            if (user == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, "teacher", teacherId));
        }

        public void CheckUserAndTeacherAreSame(UserDto userIdentity, int teacherId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            if (userIdentity.Id != teacherId)
                throw new AuthorizationException(string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.Id, teacherId));
        }

        public void CheckUserRelatesToGroup(UserDto userIdentity, List<LessonDto> lessons, int groupId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            if (CheckerRole.IsStudent(userIdentity.Roles))
            {
                var studentGroups = _groupRepository.GetGroupsByStudentId(userIdentity.Id);
                var result = studentGroups.FirstOrDefault(g => g.Id == groupId);
                if (result == default)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntRelateToEntity, userIdentity.Id, "group", groupId));
                }
            }
            else if (CheckerRole.IsTeacher(userIdentity.Roles))
            {
                var result = lessons.FirstOrDefault(l => l.Teacher.Id == userIdentity.Id);
                if (result == default )
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntRelateToEntity, userIdentity.Id, "group", groupId));
                }
            }
        }

        public void CheckUserRelatesToLesson(UserDto userIdentity, int lessonId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (CheckerRole.IsStudent(userIdentity.Roles))
            {
                var studentGroups = _groupRepository.GetGroupsByStudentId(userIdentity.Id);
                var result = studentGroups.Intersect(lesson.Groups);
                if (result == default)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntRelateToEntity, userIdentity.Id, nameof(lesson), lesson.Id));
                }
            }
            else if(CheckerRole.IsTeacher(userIdentity.Roles))
            {
                if(userIdentity.Id != lesson.Id)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntRelateToEntity, userIdentity.Id, nameof(lesson), lesson.Id));

                }
            }
        }

    }
}