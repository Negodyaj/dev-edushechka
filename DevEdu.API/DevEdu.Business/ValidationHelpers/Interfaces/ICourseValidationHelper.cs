using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICourseValidationHelper
    {
        void CheckCourseExistence(CourseDto course);
        void CourseAccessValidate(CourseDto dto, int userId);
    }
}