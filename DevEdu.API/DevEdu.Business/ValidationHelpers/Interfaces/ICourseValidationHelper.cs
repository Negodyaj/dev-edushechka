using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ICourseValidationHelper
    {
        Task<CourseDto> GetCourseByIdAndThrowIfNotFoundAsync(int courseId);
        Task CourseAccessValidateAsync(CourseDto dto, int userId);
    }
}