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

        public LessonDto CheckLessonExistence(int lessonId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lessonId));
            return lesson;
        }

        public void CheckUserAndTeacherAreSame(UserIdentityInfo userIdentity, int teacherId)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            if (userIdentity.UserId != teacherId)
                throw new ValidationException(string.Format(ServiceMessages.UserAndTeacherAreNotSame, userIdentity.UserId, teacherId));
        }

        public void CheckUserBelongsToLesson(UserIdentityInfo userIdentity, LessonDto lesson)
        {
            if (CheckerRole.IsAdmin(userIdentity.Roles))
                return;

            if (CheckerRole.IsStudent(userIdentity.Roles))
            {
                var studentGroups = _groupRepository.GetGroupsByUserId(userIdentity.UserId);
                var result = studentGroups.Where(sg => (lesson.Groups).Any(lg => lg.Id == sg.Id));
                if (result.Count() == 0)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
            else if(CheckerRole.IsTeacher(userIdentity.Roles))
            {
                if(userIdentity.UserId != lesson.Teacher.Id)
                {
                    throw new AuthorizationException(string.Format(ServiceMessages.UserDoesntBelongToLesson, userIdentity.UserId, lesson.Id));
                }
            }
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