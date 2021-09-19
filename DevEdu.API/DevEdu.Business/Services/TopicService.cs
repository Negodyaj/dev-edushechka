using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicValidationHelper _topicValidationHelper;
        private readonly ITagValidationHelper _tagValidationHelper;

        public TopicService(
            ITopicRepository topicRepository,
            ITagRepository tagRepository,
            ITopicValidationHelper topicValidationHelper,
            ITagValidationHelper tagValidationHelper
            )
        {
            _topicRepository = topicRepository;
            _topicValidationHelper = topicValidationHelper;
            _tagValidationHelper = tagValidationHelper;
        }

        public int AddTopic(TopicDto topicDto)
        {
            var topicId = _topicRepository.AddTopicAsync(topicDto);
            if (topicDto.Tags == null || topicDto.Tags.Count == 0)
                return topicId;

            topicDto.Tags.ForEach(tag => AddTagToTopic(topicId, tag.Id));
            return topicId;
        }

        public void DeleteTopic(int id)
        {
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(id);
            _topicRepository.DeleteTopicAsync(id);
        }

        public TopicDto GetTopic(int id)
        {
            var topicDto = _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(id);
            return topicDto;
        }

        public List<TopicDto> GetAllTopics()
        {
            return _topicRepository.GetAllTopicsAsync();
        }

        public TopicDto UpdateTopic(int id, TopicDto topicDto)
        {
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(id);
            topicDto.Id = id;
            _topicRepository.UpdateTopicAsync(topicDto);
            return _topicRepository.GetTopicAsync(id);
        }

        public int AddTagToTopic(int topicId, int tagId)
        {
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);
            return _topicRepository.AddTagToTopicAsync(topicId, tagId);
        }

        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            _topicValidationHelper.GetTopicByIdAndThrowIfNotFound(topicId);
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);
            return _topicRepository.DeleteTagFromTopicAsync(topicId, tagId);
        }
    }
}