using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly ITagValidationHelper _tagValidationHelper;

        public TagService(ITagRepository repository, ITagValidationHelper tagValidationHelper)
        {
            _repository = repository;
            _tagValidationHelper = tagValidationHelper;
        }

        public async Task<TagDto> AddTagAsync(TagDto dto)
        {
            dto.Id =await _repository.AddTagAsync(dto);
            return dto;
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(id);
            await _repository.DeleteTagAsync(id);
        }

        public async Task<TagDto> UpdateTagAsync(TagDto dto, int id)
        {
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(id);
            dto.Id = id;
            await _repository.UpdateTagAsync(dto);
            return await _repository.SelectTagByIdAsync(id);
        }

        public async Task<List<TagDto>> GetAllTagsAsync() => await _repository.SelectAllTagsAsync();

        public async Task<TagDto> GetTagByIdAsync(int id) => await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(id);
    }
}