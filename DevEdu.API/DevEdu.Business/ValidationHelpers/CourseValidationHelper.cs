using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class CourseValidationHelper : ICourseValidationHelper
    {
        private readonly ICourseRepository _courseRepository;

        public CourseValidationHelper(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void CheckCourseExistence(int courseId)
        {
            var course = _courseRepository.GetCourse(courseId);
            if (course == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), courseId));
        }

        //public void CheckProvidedCoursesAreUnique(List<int> courses)
        //{
        //    if(!(courses.Distinct().Count() == courses.Count()))
        //        throw new ValidationException(ServiceMessages.DuplicateCoursesValuesProvided);
        //}
    }
}