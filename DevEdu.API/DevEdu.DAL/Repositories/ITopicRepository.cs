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

        public int AddTopic(TopicDto topicDto);

        public void DeleteTopic(int id);

        public TopicDto GetTopic(int id);

        public List<TopicDto> GetAllTopic();

        public void UpdateTopic(int id, TopicDto topicDto);
    }
}
