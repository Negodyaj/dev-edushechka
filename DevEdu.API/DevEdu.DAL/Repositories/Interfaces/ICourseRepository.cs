using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ICourseRepository
    {
        Task<int> AddCourseAsync(CourseDto courseDto);
        Task DeleteCourseAsync(int id);
        Task<CourseDto> GetCourseAsync(int id);
        Task<List<CourseDto>> GetCoursesAsync();
        Task UpdateCourseAsync(CourseDto courseDto);
        Task AddTaskToCourseAsync(int courseId, int taskId);
        Task DeleteTaskFromCourseAsync(int courseId, int taskId);
        Task<List<CourseTopicDto>> SelectAllTopicsByCourseIdAsync(int courseId);
        Task DeleteAllTopicsByCourseIdAsync(int courseId);
        Task UpdateCourseTopicsByCourseId(List<CourseTopicDto> topics);
        Task<List<CourseDto>> GetCoursesToTaskByTaskIdAsync(int id);
        Task<List<CourseDto>> GetCoursesByMaterialIdAsync(int id);
        Task<int> AddCourseMaterialReferenceAsync(int courseId, int materialId);
        Task RemoveCourseMaterialReferenceAsync(int courseId, int materialId);
    }
}