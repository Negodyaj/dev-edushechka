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

        public TopicService(ITopicRepository topicRepository, ITopicValidationHelper topicValidationHelper)
        {
            _topicRepository = topicRepository;
            _topicValidationHelper = topicValidationHelper;
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
            _topicValidationHelper.CheckTopicExistence(id);
            return _topicRepository.GetTopic(id);
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

        public int AddTagToTopic(int topicId, int tagId) => _topicRepository.AddTagToTopic(topicId, tagId);

        public int DeleteTagFromTopic(int topicId, int tagId) => _topicRepository.DeleteTagFromTopic(topicId, tagId);
    }
}