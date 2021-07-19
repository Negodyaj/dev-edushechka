using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface ITopicRepository 
    {
        int AddTopic(TopicDto topicDto);
        void DeleteTopic(int id);
        List<TopicDto> GetAllTopic();
        TopicDto GetTopic(int id);
        void UpdateTopic(int id, TopicDto topicDto);
    }
}
