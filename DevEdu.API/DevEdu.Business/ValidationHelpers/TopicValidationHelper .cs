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

        public TopicDto GetTopicByIdAndThrowIfNotFound(int topicId)
        {
            var topic = _topicRepository.GetTopic(topicId);
            if (topic == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(topic), topicId));
            return topic;
        }
        public void GetTopicByListDtoAndThrowIfNotFound(List<CourseTopicDto> topics)
        {
            var topicsFromBd = _topicRepository.GetAllTopics();
            var areTopicsInDataBase = topics.All(d => topicsFromBd.Any(t => t.Id == d.Id));

            if (!areTopicsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
        }
        public CourseTopicDto GetCourseTopicByIdAndThrowIfNotFound(int id)
        {
            var courseTopic = _topicRepository.GetCourseTopicById(id);
            if (courseTopic == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(courseTopic), id));
            return courseTopic;
        }
        public List<CourseTopicDto> GetCourseTopicBySeveralIdAndThrowIfNotFound(List<int> ids)
        {
            var courseTopic = _topicRepository.GetCourseTopicBySeveralId(ids);
            var areCourseTopicsInDataBase = ids.All(d => courseTopic.Any(t => t.Id == d));
            if (!areCourseTopicsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
            return courseTopic;
         }
    }
}