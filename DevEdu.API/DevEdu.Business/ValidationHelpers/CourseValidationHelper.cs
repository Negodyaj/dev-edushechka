using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class CourseValidationHelper : ICourseValidationHelper
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;

        public CourseValidationHelper(ICourseRepository courseRepository, IGroupRepository groupRepository)
        {
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
        }

        public CourseDto GetCourseByIdAndThrowIfNotFound(int courseId)
        {
            var course = _courseRepository.GetCourse(courseId);
            if (course == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), courseId));

            return course;
        }

        public void CourseAccessValidate(CourseDto dto, int userId)
        {
            var groupsByCourse = _groupRepository.GetGroupsByCourseId(dto.Id);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByCourse.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "course", dto.Id));
        }
    }
}