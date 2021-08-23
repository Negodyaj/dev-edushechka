using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class TagValidationHelper : ITagValidationHelper
    {
        private readonly ITagRepository _tagRepository;

        public TagValidationHelper(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public TagDto GetTagByIdAndThrowIfNotFound(int tagId)
        {
            var tag = _tagRepository.SelectTagById(tagId);
            if (tag == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(tag), tagId));
            return tag;
        }
    }
}