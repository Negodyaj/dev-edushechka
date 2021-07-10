using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Servicies
{
         public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public int AddTopic(TopicDto topicDto) => _topicRepository.AddTopic(topicDto);

        public void DeleteTopic(int id) => _topicRepository.DeleteTopic(id);

        public TopicDto GetTopic(int id) => _topicRepository.GetTopic(id);

        public List<TopicDto> GetAllTopics() => _topicRepository.GetAllTopics();

        public void UpdateTopic(int id, TopicDto topicDto)
        {
            topicDto.Id = id;
            _topicRepository.UpdateTopic(topicDto);
        }
    }
}
