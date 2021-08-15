using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICourseValidationHelper
    {
        CourseDto GetCourseByIdAndThrowIfNotFound(int courseId);
        void CourseAccessValidate(CourseDto dto, int userId);
    }
}