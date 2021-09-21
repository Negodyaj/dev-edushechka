using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ICourseService
    {
        Task<CourseDto> GetCourseAsync(int id);
        Task<CourseDto> GetFullCourseInfoAsync(int id, UserIdentityInfo userToken);
        Task<CourseDto> AddCourseAsync(CourseDto courseDto);
        Task DeleteCourseAsync(int id);
        Task<List<CourseDto>> GetCoursesAsync();
        Task<CourseDto> UpdateCourseAsync(int id, CourseDto courseDto);
        Task<int> AddTopicToCourseAsync(int courseId, int topicId, CourseTopicDto dto);
        Task<List<int>> AddTopicsToCourseAsync(int courseId, List<CourseTopicDto> listDto);
        Task<List<int>> UpdateCourseTopicsByCourseIdAsync(int courseId, List<CourseTopicDto> topics);
        Task DeleteTopicFromCourseAsync(int courseId, int topicId);
        Task<List<CourseTopicDto>> SelectAllTopicsByCourseIdAsync(int courseId);
        Task DeleteTaskFromCourseAsync(int courseId, int taskId);
        Task AddTaskToCourseAsync(int courseId, int taskId);
        Task<int> AddCourseMaterialReferenceAsync(int courseId, int materialId);
        Task RemoveCourseMaterialReferenceAsync(int courseId, int materialId);
        Task<CourseTopicDto> GetCourseTopicByIdAsync(int id);
        Task<List<CourseTopicDto>> GetCourseTopicBySeveralIdAsync(List<int> ids);
    }
}