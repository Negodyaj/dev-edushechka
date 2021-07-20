using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
         public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public int AddTopic(TopicDto topicDto)
        {
            var topicId = _topicRepository.AddTopic(topicDto);
            if (topicDto.Tags == null || topicDto.Tags.Count == 0)
                return topicId;

            topicDto.Tags.ForEach(tag => AddTagToTopic(topicId, tag.Id));
            return topicId;
        } 

        public void DeleteTopic(int id) => _topicRepository.DeleteTopic(id);

        public TopicDto GetTopic(int id) => _topicRepository.GetTopic(id);

        public List<TopicDto> GetAllTopics() => _topicRepository.GetAllTopics();

        public void UpdateTopic(int id, TopicDto topicDto)
        {
            topicDto.Id = id;
            _topicRepository.UpdateTopic(topicDto);
        }

        public int AddTagToTopic(int topicId, int tagId) => _topicRepository.AddTagToTopic(topicId, tagId);

        public int DeleteTagFromTopic(int topicId, int tagId) => _topicRepository.DeleteTagFromTopic(topicId, tagId);
    }
}
