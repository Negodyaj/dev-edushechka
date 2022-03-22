using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicValidationHelper _topicValidationHelper;

        public TopicService(
            ITopicRepository topicRepository,
            ITopicValidationHelper topicValidationHelper
            )
        {
            _topicRepository = topicRepository;
            _topicValidationHelper = topicValidationHelper;
        }

        public async Task<int> AddTopicAsync(TopicDto topicDto)
        {
            return await _topicRepository.AddTopicAsync(topicDto);
        }

        public async Task DeleteTopicAsync(int id)
        {
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(id);
            await _topicRepository.DeleteTopicAsync(id);
        }

        public async Task<TopicDto> GetTopicAsync(int id)
        {
            var topicDto = await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(id);
            return topicDto;
        }

        public async Task<List<TopicDto>> GetAllTopicsAsync()
        {
            return await _topicRepository.GetAllTopicsAsync();
        }

        public async Task<TopicDto> UpdateTopicAsync(int id, TopicDto topicDto)
        {
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(id);
            topicDto.Id = id;
            await _topicRepository.UpdateTopicAsync(topicDto);
            var result = await _topicRepository.GetTopicAsync(id);

            return result;
        }
    }
}