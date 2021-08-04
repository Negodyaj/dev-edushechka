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
            var teacher = _userRepository.SelectUserById(teacherId);
            if (teacher == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(teacher), teacherId));
        }

        public void CheckUserAndTeacherAreSame(UserDto userIdentity, int teacherId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            if (userIdentity.Id != teacherId)
                throw new ValidationException(string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.Id, teacherId));
        }

        public void CheckUserBelongsToLesson(UserDto userIdentity, int lessonId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            var lesson = _lessonRepository.SelectLessonById(lessonId);

            if (CheckerRole.IsStudent(userIdentity.Roles))
            {
                var studentGroups = _groupRepository.GetGroupsByStudentId(userIdentity.Id);
                var result = studentGroups.Where(sg => (lesson.Groups).Any(lg => lg.Id == sg.Id));
                if (result.Count() == 0)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.Id, lesson.Id));
                }
            }
            else if(CheckerRole.IsTeacher(userIdentity.Roles))
            {
                if(userIdentity.Id != lesson.Teacher.Id)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.Id, lesson.Id));

                }
            }
        }

    }
}