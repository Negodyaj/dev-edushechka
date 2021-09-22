using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class TopicValidationHelper : ITopicValidationHelper
    {
        private readonly ITopicRepository _topicRepository;

        public TopicValidationHelper(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<TopicDto> GetTopicByIdAndThrowIfNotFoundAsync(int topicId)
        {
            var topic = await _topicRepository.GetTopicAsync(topicId);
            if (topic == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(topic), topicId));
            return topic;
        }

        public async Task GetTopicByListDtoAndThrowIfNotFoundAsync(List<CourseTopicDto> topics)
        {
            var topicsFromBd = await _topicRepository.GetAllTopicsAsync();
            var topicsIdsFromBd = topicsFromBd.Select(t => t.Id).ToList();
            var topicIdsFromParametrs = topics.Select(t => t.Topic.Id).ToList();

            var areTopicsInDataBase = topicsIdsFromBd.Intersect(topicIdsFromParametrs).ToList();

            if (areTopicsInDataBase.Count() != topicIdsFromParametrs.Count())
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }
        }

        public async Task<CourseTopicDto> GetCourseTopicByIdAndThrowIfNotFoundAsync(int id)
        {
            var courseTopic = await _topicRepository.GetCourseTopicByIdAsync(id);
            if (courseTopic == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(courseTopic), id));
            
            return courseTopic;
        }

        public async Task<List<CourseTopicDto>> GetCourseTopicBySeveralIdAndThrowIfNotFoundAsync(List<int> ids)
        {
            var courseTopic = await _topicRepository.GetCourseTopicBySeveralIdAsync(ids);
            var areCourseTopicsInDataBase = ids.All(d => courseTopic.Any(t => t.Id == d));
            
            if (!areCourseTopicsInDataBase)
            {
                throw new EntityNotFoundException(ServiceMessages.EntityNotFound);
            }

            return courseTopic;
        }
    }
}