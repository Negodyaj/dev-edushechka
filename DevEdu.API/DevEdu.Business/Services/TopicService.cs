using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITagRepository _tagRepository;
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
            _tagRepository = tagRepository;
            _topicValidationHelper = topicValidationHelper;
            _tagValidationHelper = tagValidationHelper;
        }

        public int AddTopic(TopicDto topicDto)
        {       
            var topicId = _topicRepository.AddTopic(topicDto);            
                if (topicDto.Tags == null || topicDto.Tags.Count == 0)
                    return topicId;

                topicDto.Tags.ForEach(tag => AddTagToTopic(topicId, tag.Id));            
                return topicId;
        }

        public void DeleteTopic(int id)
        {              
            _topicValidationHelper.CheckTopicExistence(id);            
                _topicRepository.DeleteTopic(id);            
        }

        public TopicDto GetTopic(int id)
        {
           var topicDto= _topicValidationHelper.CheckTopicExistence(id);
            return topicDto;
        }

        public List<TopicDto> GetAllTopics()
        {
            return _topicRepository.GetAllTopics();
        } 

        public TopicDto UpdateTopic(int id, TopicDto topicDto)
        {
            _topicValidationHelper.CheckTopicExistence(id);            
            topicDto.Id = id;
            _topicRepository.UpdateTopic(topicDto);
            return _topicRepository.GetTopic(id);
        }

        public int AddTagToTopic(int topicId, int tagId)
        {
            _topicValidationHelper.CheckTopicExistence(topicId);
            _tagValidationHelper.CheckTagExistence(tagId);
            return _topicRepository.AddTagToTopic(topicId, tagId);
        }

        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            _topicValidationHelper.CheckTopicExistence(topicId);
            _tagValidationHelper.CheckTagExistence(tagId);
            return _topicRepository.DeleteTagFromTopic(topicId, tagId);
        }
    }
}