using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITopicValidationHelper
    {
        Task<TopicDto> GetTopicByIdAndThrowIfNotFoundAsync(int topicId);
        Task GetTopicByListDtoAndThrowIfNotFoundAsync(List<CourseTopicDto> topics);
        Task<CourseTopicDto> GetCourseTopicByIdAndThrowIfNotFoundAsync(int id);
        Task<List<CourseTopicDto>> GetCourseTopicBySeveralIdAndThrowIfNotFoundAsync(List<int> ids);
    }
}