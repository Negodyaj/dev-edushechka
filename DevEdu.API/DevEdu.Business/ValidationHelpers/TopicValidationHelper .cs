using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

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
    }
}