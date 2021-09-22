using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ITopicRepository
    {
        Task<int> AddTopicAsync(TopicDto topicDto);
        Task DeleteTopicAsync(int id);
        Task<List<TopicDto>> GetAllTopicsAsync();
        Task<TopicDto> GetTopicAsync(int id);
        Task<int> UpdateTopicAsync(TopicDto topicDto);
        Task<int> AddTopicToCourseAsync(CourseTopicDto dto);
        Task<List<int>> AddTopicsToCourseAsync(List<CourseTopicDto> dto);
        Task DeleteTopicFromCourseAsync(int courseId, int topicId);
        Task<List<TopicDto>> GetTopicsByCourseIdAsync(int courseId);
        Task<int> AddTagToTopicAsync(int topicId, int tagId);
        Task<int> DeleteTagFromTopicAsync(int topicId, int tagId);
        Task<CourseTopicDto> GetCourseTopicByIdAsync(int id);
        Task<List<CourseTopicDto>> GetCourseTopicBySeveralIdAsync(List<int> ids);
    }
}