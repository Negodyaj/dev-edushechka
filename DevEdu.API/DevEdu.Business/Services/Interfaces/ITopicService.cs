using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ITopicService
    {
        Task<int> AddTopicAsync(TopicDto topicDto);
        Task DeleteTopicAsync(int id);
        Task<List<TopicDto>> GetAllTopicsAsync();
        Task<TopicDto> GetTopicAsync(int id);
        Task<TopicDto> UpdateTopicAsync(int id, TopicDto topicDto);
    }
}