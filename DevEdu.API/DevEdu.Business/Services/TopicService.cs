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

        public async Task<int> AddTopicAsync(TopicDto topicDto)
        {
            var topicId = await _topicRepository.AddTopicAsync(topicDto);
            if (topicDto.Tags == null ||
                topicDto.Tags.Count == 0)
                return topicId;

            topicDto.Tags.ForEach(
                 async tag => await AddTagToTopicAsync(topicId, tag.Id));

            return topicId;
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

        public async Task<int> AddTagToTopicAsync(int topicId, int tagId)
        {
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topicId);
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(tagId);
            var resilt = await _topicRepository.AddTagToTopicAsync(topicId, tagId);
            return resilt;
        }

        public async Task<int> DeleteTagFromTopicAsync(int topicId, int tagId)
        {
            await _topicValidationHelper.GetTopicByIdAndThrowIfNotFoundAsync(topicId);
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(tagId);
            var result = await _topicRepository.DeleteTagFromTopicAsync(topicId, tagId);
            return result;
        }
    }
}