using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class TagValidationHelper : ITagValidationHelper
    {
        private readonly ITagRepository _tagRepository;

        public TagValidationHelper(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<TagDto> GetTagByIdAndThrowIfNotFoundAsync(int tagId)
        {
            var tag = await _tagRepository.SelectTagByIdAsync(tagId);
            if (tag == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(tag), tagId));
            
            return tag;
        }
    }
}