using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface ITopicService
    {
        int AddTopic(TopicDto topicDto);
        void DeleteTopic(int id);
        List<TopicDto> GetAllTopics();
        TopicDto GetTopic(int id);
        TopicDto UpdateTopic(int id, TopicDto topicDto);
        int AddTagToTopic(int topicId, int tagId);
        int DeleteTagFromTopic(int topicId, int tagId);
    }
}