using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

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

        public TagDto AddTag(TagDto dto)
        {
            dto.Id = _repository.AddTagAsync(dto);
            return dto;
        }

        public void DeleteTag(int id)
        {
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(id);
            _repository.DeleteTagAsync(id);
        }

        public TagDto UpdateTag(TagDto dto, int id)
        {
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(id);
            dto.Id = id;
            _repository.UpdateTagAsync(dto);
            return _repository.SelectTagByIdAsync(id);
        }

        public List<TagDto> GetAllTags() => _repository.SelectAllTagsAsync();

        public TagDto GetTagById(int id) => _tagValidationHelper.GetTagByIdAndThrowIfNotFound(id);
    }
}