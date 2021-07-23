using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.Exceptions;

namespace DevEdu.Business.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITagRepository _tagRepository;

        public TopicService(
            ITopicRepository topicRepository,
            ITagRepository tagRepository
            )
        {
            _topicRepository = topicRepository;
            _tagRepository = tagRepository;
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

        public int AddTagToTopic(int topicId, int tagId)
        {
            var topic = _topicRepository.GetTopic(topicId);
            if (topic == default)
                throw new EntityNotFoundException($"topic with id = {topicId} was not found");
            var tag = _tagRepository.SelectTagById(tagId);
            if (tag == default)
                throw new EntityNotFoundException($"tag with id = {tagId} was not found");
            return _topicRepository.AddTagToTopic(topicId, tagId);
        }

        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            var topic = _topicRepository.GetTopic(topicId);
            if (topic == default)
                throw new EntityNotFoundException($"topic with id = {topicId} was not found");
            var tag = _tagRepository.SelectTagById(tagId);
            if (tag == default)
                throw new EntityNotFoundException($"tag with id = {tagId} was not found");
            return _topicRepository.DeleteTagFromTopic(topicId, tagId);
        }
    }
}
