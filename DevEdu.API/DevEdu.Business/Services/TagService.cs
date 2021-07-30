using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
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

        public int AddTag(TagDto dto) => _repository.AddTag(dto);

        public void DeleteTag(int id)
        {
            _tagValidationHelper.CheckTagExistence(id);
            _repository.DeleteTag(id);
        }

        public TagDto UpdateTag(TagDto dto, int id)
        {
            _tagValidationHelper.CheckTagExistence(id);
            dto.Id = id;
            _repository.UpdateTag(dto);
            return _repository.SelectTagById(id);
        }

        public List<TagDto> GetAllTags() => _repository.SelectAllTags();

        public TagDto GetTagById(int id)
        {
            var dto = _repository.SelectTagById(id);
            if (dto == default)
            {
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(dto), id));
            }
            return dto;
        }
    }
}