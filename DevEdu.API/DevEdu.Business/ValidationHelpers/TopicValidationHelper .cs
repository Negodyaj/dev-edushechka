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
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(topic), topicId));
        }
        public void CheckTopicsExistence(List<CourseTopicDto> topics)
        {
            var topicsFromBd = _topicRepository.GetAllTopics();
            var areTopicsInDataBase = topics.All(d => topicsFromBd.Any(t => t.Id == d.Id));

            if (!areTopicsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
        }
    }
}