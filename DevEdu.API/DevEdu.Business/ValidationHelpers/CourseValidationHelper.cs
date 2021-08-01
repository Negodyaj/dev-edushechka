using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class CourseValidationHelper : ICourseValidationHelper
    {
        private readonly ICourseRepository _courseRepository;

        public CourseValidationHelper(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void CheckCourseExistence(CourseDto course)
        {
            if (course == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), course.Id));
        }

        public void CourseAccessValidate(CourseDto dto, int userId)
        {
            var course = dto;
            if (course == default)
                throw new AuthorizationException(string.Format(ServiceMessages.UserHasNoAccessMessage, userId));
        }
    }
}