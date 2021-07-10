using DevEdu.DAL.Repositories;


namespace DevEdu.Business.Servicies
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public int AddCourseMaterialReference(int courseId, int materialId) => _courseRepository.AddCourseMaterialReference(courseId, materialId);

        public int RemoveCourseMaterialReference(int courseId, int materialId) => _courseRepository.RemoveCourseMaterialReference(courseId, materialId);

    }
}
