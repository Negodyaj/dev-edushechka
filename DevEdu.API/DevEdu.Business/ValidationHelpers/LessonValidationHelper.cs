using System.Linq;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;

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

        public void CheckLessonExistence(int lessonId)
        {
            var lesson = _lessonRepository.SelectLessonById(lessonId);
            if (lesson == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lessonId));
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