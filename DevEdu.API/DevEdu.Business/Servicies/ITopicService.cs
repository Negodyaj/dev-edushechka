using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
    public interface ITopicService
    {
        int AddTopic(TopicDto topicDto);
        void DeleteTopic(int id);
        List<TopicDto> GetAllTopics();
        TopicDto GetTopic(int id);
        void UpdateTopic(int id, TopicDto topicDto);
    }
}