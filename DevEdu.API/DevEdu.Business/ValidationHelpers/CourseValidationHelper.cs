using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<CourseDto> GetCourseByIdAndThrowIfNotFoundAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseAsync(courseId);
            if (course == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), courseId));

            return course;
        }

        public async Task CourseAccessValidateAsync(CourseDto dto, int userId)
        {
            var groupsByCourse = await _groupRepository.GetGroupsByCourseIdAsync(dto.Id);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);

            var result = groupsByCourse.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "course", dto.Id));
        }
    }
}