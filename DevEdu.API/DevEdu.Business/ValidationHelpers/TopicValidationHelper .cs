using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class TopicValidationHelper : ITopicValidationHelper
    {
        private readonly ITopicRepository _topicRepository;

        public TopicValidationHelper(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public void CheckTopicExistence(int topicId)
        {
            var topic = _topicRepository.GetTopic(topicId);
            if (topic == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(topic), topicId));
        }
        public void CheckTopicsExistence(List<CourseTopicDto> topics)
        {
            var topicsFromBd = _topicRepository.GetAllTopics();
            var exsitedTopicIds = topics.Select(s => s.Topic.Id).Where(d => !topicsFromBd.Select(g => g.Id).Contains(d));
            if (exsitedTopicIds.Count() > 0)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
        }
    }
}