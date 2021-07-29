using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;

        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }

        public int AddTag(TagDto dto) => _repository.AddTag(dto);

        public void DeleteTag(int id) => _repository.DeleteTag(id);

        public TagDto UpdateTag(TagDto dto, int id)
        {
            dto.Id = id;
            _repository.UpdateTag(dto);
            return GetTagById(id);
        }

        public List<TagDto> GetAllTags() => _repository.SelectAllTags();

        public TagDto GetTagById(int id) => _repository.SelectTagById(id);
    }
}